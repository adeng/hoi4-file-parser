using HoI4Parser.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HoI4Parser.Services
{
    public static class OutputService
    {
        private static int[] YEARS = { 1918, 1936, 1937, 1938, 1939, 1940, 1941, 1942, 1943, 1944, 1945, 1946, 1947, 1948 };

        public static void GetTransformedRegiments()
        {
            DataTable needs = DataService.GetRegimentNeeds();
            DataTable regiments = DataService.GetCalculatedRegiments();

            Dictionary<int, Dictionary<string, RegimentStatistics>> regimentsByYear = new Dictionary<int, Dictionary<string, RegimentStatistics>>();

            Dictionary<string, string> translationTable = new Dictionary<string, string>();
            Dictionary<string, long> minYear = DataService.GetEarliestEquipmentYears()
                .AsEnumerable()
                .ToDictionary<DataRow, string, long>(row => (string)row[0], row => (long)row[1]);

            for (int i = 0; i < YEARS.Count(); i++)
            {
                int year = YEARS[i];

                var validRegiments = minYear.Where(pair => pair.Value <= year).Select(pair => pair.Key).ToList();
                for (int j = 0; j < validRegiments.Count(); j++)
                {
                    string regimentId = validRegiments[j];
                    Dictionary<string, bool> checklist = needs.AsEnumerable()
                        .Where(row => (string)row[0] == regimentId)
                        .ToDictionary<DataRow, string, bool>(row => (string)row[1], row => false);

                    // Initialize dictionary
                    if (!regimentsByYear.ContainsKey(year))
                        regimentsByYear.Add(year, new Dictionary<string, RegimentStatistics>());

                    // Plug gaps for equipment archetypes
                    foreach (string archetype in needs.AsEnumerable().Where(row => (string)row[0] == regimentId).Select(row => (string)row[1]))
                    {
                        // Get max year for equipment less than current year
                        long equipYear = regiments.AsEnumerable()
                            .Where(row => (string)row[18] == archetype && (long)row[7] <= year && (string)row[0] == regimentId)
                            .Select(row => (long)row[7])
                            .Max();

                        var filteredEquipRow = regiments.AsEnumerable()
                            .Where(row => (string)row[18] == archetype && (long)row[7] == equipYear && (string)row[0] == regimentId);

                        // COME BACK TO THIS WHEN YOU IMPLEMENT EQUIPMENT VARIATIONS
                        //if (filteredEquipRow.Count() > 1)
                        //    throw new Exception("Too many rows!");

                        var row = filteredEquipRow.Last();

                        if (!regimentsByYear[year].ContainsKey(regimentId))
                        {
                            RegimentStatistics regiment = new RegimentStatistics(
                                (string)row[0],
                                (double)row[5],
                                (double)row[6],
                                (long)row[7],
                                (double)row[8],
                                (double)row[9],
                                (double)row[10],
                                (double)row[11],
                                (double)row[12],
                                (double)row[13],
                                (double)row[14],
                                (double)row[15],
                                (long)row[16],
                                (double)row[17]
                            );

                            regimentsByYear[year].Add(regimentId, regiment);
                            checklist[(string)row[18]] = true;
                        }
                        else
                        {
                            regimentsByYear[year][regimentId].HP += (double)row[5];
                            regimentsByYear[year][regimentId].Organization += (double)row[6];
                            regimentsByYear[year][regimentId].SoftAttack += (double)row[8];
                            regimentsByYear[year][regimentId].HardAttack += (double)row[9];
                            regimentsByYear[year][regimentId].Breakthrough += (double)row[10];
                            regimentsByYear[year][regimentId].Defense += (double)row[11];
                            regimentsByYear[year][regimentId].Piercing += (double)row[12];
                            regimentsByYear[year][regimentId].AirAttack += (double)row[13];
                            regimentsByYear[year][regimentId].Armor = Math.Max(regimentsByYear[year][regimentId].Armor, (double)row[14]);
                            regimentsByYear[year][regimentId].Cost += (double)row[15];

                            checklist[(string)row[18]] = true;
                        }
                    }
                }
            }

            JObject obj = JObject.FromObject(regimentsByYear);
            FileService.WriteJSON(obj, "transformed_regiments.json");
        }

        public static void GetRegiments()
        {
            DataTable regiments = DataService.GetRegiments();
            JArray results = new JArray();
            JObject current = new JObject();
            JArray equipment = new JArray();

            string currentRegiment = "";

            foreach(DataRow row in regiments.AsEnumerable())
            {
                if((string)row[0] != currentRegiment)
                {
                    // Create and push JObject
                    if(currentRegiment != "")
                    {
                        current.Add(new JProperty("equipment", equipment));
                        results.Add(current);
                        equipment.Clear();
                    }

                    current = JObject.FromObject(new
                    {
                        regiment_id = (string)row[0],
                        regiment_name = (string)(row[1].GetType() == typeof(System.DBNull) ? "" : row[1]),
                        hp = (double)row[2],
                        organization = (double)row[3],
                        priority = (long)row[4],
                        type = (string)row[8],
                        same_support_type = (string)row[9],
                        width = (long)row[10]
                    });

                    currentRegiment = (string)row[0];
                }

                JObject need = JObject.FromObject(new
                {
                    archetype_id = (string)row[5],
                    archetype_name = (string)row[6],
                    number = (long)row[7]
                });

                if(!equipment.Where(x => (string)x["archetype_id"] == (string)need["archetype_id"]).Any())
                    equipment.Add(need);
            }

            current.Add(new JProperty("equipment", equipment));
            results.Add(current);

            FileService.WriteJSON(results, "regiments.json");
        }

        public static void GetTerrainModifiers()
        {
            DataTable terrain = DataService.GetTerrainModifiers();
            JArray results = new JArray();
            JArray regiments = new JArray();
            JObject current = new JObject();

            string currentTerrain = "";

            foreach(DataRow row in terrain.AsEnumerable())
            {
                if((string)row[0] != currentTerrain)
                {
                    if(currentTerrain != "")
                    {
                        current.Add(new JProperty("regiments", regiments));
                        results.Add(current);
                    }

                    current = JObject.FromObject(new
                    {
                        terrain = (string)row[0]
                    });

                    regiments = new JArray();
                    currentTerrain = (string)row[0];
                }

                regiments.Add(JObject.FromObject(new
                {
                    regiment_id = (string)row[1],
                    attack = (double)row[2],
                    defense = (double)row[3],
                    movement = (double)row[4]
                }));
            }

            current.Add(new JProperty("regiments", regiments));
            results.Add(current);

            FileService.WriteJSON(results, "terrain.json");
        }

        public static void GetEquipment()
        {
            DataTable equipment = DataService.GetEquipment();
            JArray results = new JArray();
            JArray matches = new JArray();
            JObject current = new JObject();

            string currentArchetype = "";

            foreach(DataRow row in equipment.AsEnumerable())
            {
                if ((string)row[0] != currentArchetype)
                {
                    // Create and push JObject
                    if (currentArchetype != "")
                    {
                        current.Add(new JProperty("equipment", matches));
                        results.Add(current);
                        matches.Clear();
                    }

                    current = JObject.FromObject(new
                    {
                        archetype_id = (string)row[0],
                        archetype_name = (string)row[1]
                    });

                    currentArchetype = (string)row[0];
                }

                JObject equip = JObject.FromObject(new
                {
                    equipment_id = (string)row[2],
                    equipment_name = (string)row[3],
                    year = (long)row[4],
                    reliability = (double)row[5],
                    max_speed = (double)row[6],
                    defense = (double)row[7],
                    breakthrough = (double)row[8],
                    soft_attack = (double)row[9],
                    hard_attack = (double)row[10],
                    piercing = (double)row[11],
                    air_attack = (double)row[12],
                    hardness = (double)row[13],
                    fuel_consumption = (double)row[14],
                    cost = (double)row[15],
                    armor = (double)row[16]
                });

                matches.Add(equip);
            }

            current.Add(new JProperty("equipment", matches));
            results.Add(current);

            FileService.WriteJSON(results, "equipment.json");
        }
    }
}

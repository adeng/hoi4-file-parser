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
            DataTable regiments = DataService.GetRegiments();

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
            //foreach(var pair in regimentsByYear)
            //{
            //    JObject stats = new JObject();
            //    foreach(var regiment in pair.Value)
            //    {
            //        stats.Add(regiment.Key, JObject.FromObject(regiment.Value));
            //    }

            //    obj.Add(new JProperty(pair.Key.ToString(), stats));
            //}

            FileService.WriteRegimentJSON(obj);
        }
    }
}

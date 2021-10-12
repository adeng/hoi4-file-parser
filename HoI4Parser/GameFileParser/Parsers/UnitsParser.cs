using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using HoI4Parser.Services;
using HoI4Parser.Models;

namespace HoI4Parser.Parsers
{
    class UnitsParser
    {
        /// <summary>
        /// Iterate over all equipment paths.
        /// </summary>
        /// <param name="path"></param>
        public static void LoadEquipment(string path)
        {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);

            // Iterate over all files in the directory
            //List<EquipmentFamily> equipments = new List<EquipmentFamily>();
            for (int i = files.Length - 1; i >= 0; i--)
            {
                // Skip ships and planes for now
                if (files[i].Contains("ship") || files[i].Contains("airframe") || files[i].Contains("convoys"))
                    continue;

                // Iterate over equipment
                using (FileStream fs = new FileStream(files[i], FileMode.Open))
                {
                    Console.WriteLine(files[i]);
                    EquipmentFamily file = ParadoxParser.Parse(fs, new EquipmentFamily());
                    DataService.WriteLandEquipment(file);

                    for (int j = file.EquipmentList.Count - 1; j >= 0; j--)
                    {
                        DataService.EquipmentDictionary.Add(file.EquipmentList[j].ID, file.EquipmentList[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Iterate over all regiment paths
        /// </summary>
        /// <param name="path"></param>
        public static void LoadRegiments(string path)
        {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);
            List<string> excludes = new List<string>
            {
                "air.txt",
                "battlecruiser.txt",
                "battleship.txt",
                "carrier.txt",
                "destroyer.txt",
                "heavy_cruiser.txt",
                "light_cruiser.txt",
                "submarine.txt"
            };

            // Iterate over all files in the directory
            //List<EquipmentFamily> equipments = new List<EquipmentFamily>();
            for (int i = files.Length - 1; i >= 0; i--)
            {
                // Skip ships and planes for now
                if (excludes.Contains(Path.GetFileName(files[i])))
                    continue;

                // Iterate over equipment
                using (FileStream fs = new FileStream(files[i], FileMode.Open))
                {
                    Console.WriteLine(files[i]);
                    RegimentFamily file = ParadoxParser.Parse(fs, new RegimentFamily());
                    DataService.WriteRegiment(file);

                    for(int j = file.RegimentList.Count - 1; j >= 0; j--)
                    {
                        DataService.RegimentDictionary.Add(file.RegimentList[j].ID, file.RegimentList[j]);
                    }
                }
            }
        }
    }
}

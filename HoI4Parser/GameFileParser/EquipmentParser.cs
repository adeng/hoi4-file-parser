using GameFileParser.Models;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameFileParser
{
    class EquipmentParser
    {
        /// <summary>
        /// Iterate over all equipment paths.
        /// </summary>
        /// <param name="path"></param>
        public static void LoadEquipment(string path)
        {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);
            List<EquipmentFamily> equipments = new List<EquipmentFamily>();

            // Iterate over all files in the directory
            for (int i = files.Length - 1; i >= 0; i--)
            {
                // Skip ships and planes for now
                if (files[i].Contains("ship") || files[i].Contains("airframe"))
                    continue;

                // Iterate over equipment
                using (FileStream fs = new FileStream(files[i], FileMode.Open))
                {
                    Console.WriteLine(files[i]);
                    EquipmentFamily file = ParadoxParser.Parse(fs, new EquipmentFamily());
                    equipments.Add(file);
                }
            }
        }
    }
}

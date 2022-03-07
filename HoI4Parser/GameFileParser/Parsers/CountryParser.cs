using HoI4Parser.Models;
using HoI4Parser.Services;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace HoI4Parser.Parsers
{
    class CountryParser
    {
        /// <summary>
        /// Iterate over country tags files to capture the list of tags
        /// </summary>
        /// <param name="path"></param>
        public static void LoadCountryTags(string path)
        {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);
            List<Country> countries = new List<Country>();

            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetFileName(files[i]) == "zz_dynamic_countries.txt")
                    continue;

                string[] lines = File.ReadAllLines(files[i]);
                foreach (string line in lines)
                {
                    string tag = line.Substring(0, 3);
                    string countrySubPath = line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\"") - line.IndexOf("\"") - 1).Replace("/", "\\");
                    string name = countrySubPath.Substring(countrySubPath.IndexOf("\\") + 1, countrySubPath.IndexOf(".txt") - countrySubPath.IndexOf("\\") - 1);

                    string countryPath = Path.GetDirectoryName(path) + "\\" + countrySubPath;

                    using (FileStream fs = new FileStream(countryPath, FileMode.Open))
                    {
                        Console.WriteLine(countryPath);
                        CountryStyle style = ParadoxParser.Parse(fs, new CountryStyle());

                        Color color = Color.FromArgb(style.ColorCodes[0], style.ColorCodes[1], style.ColorCodes[2]);
                        string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

                        Country country = new Country(name, tag, hex);
                        countries.Add(country);
                    }
                }
            }

            DataService.WriteCountries(countries);
        }

        public static void LoadAIStrategies(string path)
        {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);
            List<StrategyPlanShell> list = new List<StrategyPlanShell>();

            for (int i = files.Length - 1; i >= 0; i--)
            {
                // Iterate over AI plans
                using (FileStream fs = new FileStream(files[i], FileMode.Open))
                {
                    Console.WriteLine(files[i]);
                    StrategyPlanShell file = ParadoxParser.Parse(fs, new StrategyPlanShell());

                    DataService.WriteStrategy(file);
                }
            }
        }
    }
}

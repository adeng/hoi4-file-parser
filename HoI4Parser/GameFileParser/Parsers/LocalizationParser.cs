using HoI4Parser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoI4Parser.Parsers
{
    public static class LocalizationParser
    {
        public static void LoadLocalizationFolder(string path)
        {
            List<Tuple<string, string>> results = new List<Tuple<string, string>>();
            string[] files = Directory.GetFiles(path, "*.yml", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; i++)
            {
                var localizations = LoadYMLFile(files[i]);
                results.AddRange(localizations);
            }

            DataService.WriteLocalizations(results);
        }

        public static List<Tuple<string, string>> LoadYMLFile(string filename)
        {
            Console.WriteLine(filename);

            List<Tuple<string, string>> output = new List<Tuple<string, string>>();
            string[] lines = File.ReadAllLines(filename);
            foreach (string l in lines)
            {
                string line = l.Trim();

                // Skip unnecessary lines
                if (line.Trim().Length == 0)
                    continue;

                if (line.Trim().ToCharArray()[0] == '#')
                    continue;

                if (line == "l_english:")
                    continue;

                // Read actual data
                string[] split = line.Split(":");
                string key = split[0].Trim();
                string value;

                if (split.Length != 2)
                {
                    string[] shorter = new string[split.Length - 1];
                    for(int i = 1; i < split.Length; i++)
                    {
                        shorter[i - 1] = split[i];
                    }
                    string combined = string.Join(":", shorter).Trim();
                    value = combined.Substring(combined.IndexOf("\"") + 1, combined.LastIndexOf("\"") - combined.IndexOf("\"") - 1);
                }
                else
                {
                    string temp = split[1].Substring(1).Trim();
                    value = temp.Substring(temp.IndexOf("\"") + 1, temp.LastIndexOf("\"") - temp.IndexOf("\"") - 1);
                }

                output.Add(new Tuple<string, string>(key, value));
            }

            return output;
        }
    }
}

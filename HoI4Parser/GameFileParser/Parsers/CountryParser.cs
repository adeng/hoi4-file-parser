using System;
using System.Collections.Generic;
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

            for(int i = 0; i < files.Length; i++)
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
                    }
                }
            }
        }

        public static void LoadCountryColors(string path)
        {

        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoI4Parser.Services
{
    public static class FileService
    {
        public static void WriteRegimentJSON(JObject results)
        {
            if (File.Exists("regiments.json"))
                File.Delete("regiments.json");

            File.WriteAllText("regiments.json", results.ToString());
        }
    }
}

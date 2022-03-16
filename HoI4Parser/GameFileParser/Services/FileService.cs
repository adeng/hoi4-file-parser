using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoI4Parser.Services
{
    public static class FileService
    {
        public static void WriteJSON(JObject results, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            File.WriteAllText(filename, results.ToString());
        }

        public static void WriteJSON(JArray results, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            File.WriteAllText(filename, results.ToString());
        }
    }
}

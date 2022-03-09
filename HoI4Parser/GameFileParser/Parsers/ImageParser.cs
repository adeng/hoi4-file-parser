using HoI4Parser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pfim;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using HoI4Parser.Services;
using System.Linq;

namespace HoI4Parser.Parsers
{
    public static class ImageParser
    {
        public static List<string> StrategyPlanNameList;

        public static Dictionary<string, string> StrategyPlanMatches = new Dictionary<string, string>
        {
            { "YUG_historical", "YUG_neutrality" },
            { "USA_historical_plan", "USA_democratic" },
            { "USA_alternate_plan_1", "USA_democratic" },
            { "TUR_historical_plan", "TUR_neutrality" },
            { "TUR_balkan_pact_plan", "TUR_neutrality" },
            { "TUR_alternate_kemalist_plan", "TUR_kemalist" },
            { "TUR_ottoman_plan", "OTT_SULTANATE" },
            { "SPR_historical_plan_war_with_axis", "SPR_nationalist_spain_neutrality" },
            { "SPR_historical_plan_war_with_allies", "SPR_nationalist_spain_neutrality" },
            { "SPR_historical_plan", "SPR_democratic" },
            { "SPD_alternate_plan_2", "SPR_communism" },
            { "SPD_alternate_plan", "SPR_democratic" },
            { "SPC_alternate_plan_2", "SPR_communism" },
            { "SPC_alternate_plan", "SPR_anarchist_spain_neutrality" },
            { "SPB_alternate_plan", "SPR_carlist_spain_neutrality" },
            { "SPA_alternate_plan_2", "SPA_directory_neutrality" },
            { "SPA_alternate_plan", "SPR_nationalist_spain_neutrality" },
            { "SOV_historical_plan", "SOV_communism" },
            { "SOV_exiles_fascist_plan", "SOV_fascism" },
            { "SOV_exiles_tsarist_plan", "SOV_neutrality" },
            { "SOV_right_opposition_cooperative_plan", "SOV_right_opposition_communism" },
            { "SOV_right_opposition_plan", "SOV_right_opposition_communism" },
            { "SOV_left_opposition_cooperative_plan", "SOV_left_opposition_communism" },
            { "SOV_left_opposition_plan", "SOV_left_opposition_communism" },
            { "SAF_historical", "SAF_democratic" },
            { "ROM_historical", "ROM_democratic" },
            { "ROM_dominance", "ROM_neutrality" },
            { "ROM_allies", "ROM_democratic" },
            { "RAJ_historical", "RAJ_UK" },
            { "PRC_historical_plan", "PRC_proclaimed_communism" },
            { "POR_historical_plan", "POR_democratic" },
            { "POR_neutrality_monarchist_plan", "POR_empire_neutrality" },
            { "POR_fascist_fifth_empire_plan", "POR_empire_fascism" },
            { "POR_fascist_axis_plan", "POR_fascism" },
            { "POL_historical_plan", "POL_democratic" },
            { "POL_NSB_independent_fascist_plan", "POL_fascism" },
            { "POL_NSB_fascist_plan", "POL_fascism" },
            { "POL_monarchy_habsburg_plan", "POL_KINGDOM_neutrality" },
            { "POL_monarchy_bermondtian_plan", "POL_KINGDOM_neutrality" },
            { "POL_monarchy_romania_plan", "POL_ROM_UNION_neutrality" },
            //{ "POL_monarchy_commonwealth_plan",  }

        };

        public static void LoadFlags(string path, string size)
        {
            string[] files = Directory.GetFiles(path, "*.tga", SearchOption.TopDirectoryOnly);
            List<FlagImage> flagImages = new List<FlagImage>();
            HashSet<string> fileNames = new HashSet<string>();

            for(int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);

                string imageString = GetFlagImage(files[i]);
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string tag;

                if (fileName.Length == 3)
                    tag = fileName;
                else if (fileName.IndexOf("_") == 3)
                    tag = fileName.Substring(0, 3);
                else
                    tag = String.Empty;

                FlagImage image = new FlagImage(tag, fileName, imageString, size);

                flagImages.Add(image);
                fileNames.Add(fileName);
            }

            DataService.WriteFlagImages(flagImages);

            if(size == "full")
                LoadPlanFlagRelationships(flagImages);
        }

        public static void LoadPlanFlagRelationships(List<FlagImage> flagImages)
        {
            List<Tuple<string, string>> matches = new List<Tuple<string, string>>();

            for(int i = 0; i < StrategyPlanNameList.Count; i++)
            {
                string id = StrategyPlanNameList[i];
                string tag = id.Substring(0, 3);
                string guess = "";

                if (id.Contains("fascist"))
                    guess = $"{tag}_fascist";
                else if (id.Contains("democratic"))
                    guess = $"{tag}_democratic";
                else if (id.Contains("communist"))
                    guess = $"{tag}_communist";
                else if (id.Contains("neutrality"))
                    guess = $"{tag}_neutrality";

                if (StrategyPlanMatches.ContainsKey(id))
                    // Hand keyed previously
                    matches.Add(new Tuple<string, string>(id, StrategyPlanMatches[id]));
                else if (guess != "" && flagImages.Where(image => image.FileName == guess).Any())
                    // Best match algo
                    matches.Add(new Tuple<string, string>(id, guess));
                else if (guess != "" && flagImages.Where(image => image.FileName == tag).Any())
                    // Filename is generic flag
                    matches.Add(new Tuple<string, string>(id, tag));
                else 
                    // Don't know what else to do
                    matches.Add(new Tuple<string, string>(id, null));
            }
        }

        public static string GetFlagImage(string path)
        {
            using (var image = Pfim.Pfim.FromFile(path))
            {
                PixelFormat format;

                switch (image.Format)
                {
                    case Pfim.ImageFormat.Rgba32:
                        format = PixelFormat.Format32bppArgb;
                        break;
                    case Pfim.ImageFormat.Rgb24:
                        format = PixelFormat.Format24bppRgb;
                        break;
                    case Pfim.ImageFormat.Rgba16:
                    case Pfim.ImageFormat.R5g5b5:
                        format = PixelFormat.Format16bppArgb1555;
                        break;
                    default:
                        // see the sample for more details
                        throw new NotImplementedException();
                }

                try
                {
                    var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                    var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, data);

                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageArray = ms.ToArray();
                    return Convert.ToBase64String(imageArray);
                }
                catch
                {
                    throw new Exception();
                }
            }
        }
    }
}

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
            { "SPD_historical_plan", "SPR_democratic" },
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
            { "POL_monarchy_commonwealth_plan", "plc_unified_neutrality" },
            { "NZL_historical", "NZL_democratic" },
            { "mexico_fascist_latin_american_order", "MEX_cristero_fascism" },
            { "mexico_social_catholicism", "MEX_cristero_democratic" },
            { "mexico_historical", "MEX_neutrality" },
            { "MAN_historical_plan", "MAN" },
            { "MAN_default_plan", "man_restored" },
            { "LIT_historical", "LIT_neutrality" },
            { "LIT_monarchist", "plc_unified_neutrality" },
            { "LAT_entente", "bal_unified_democratic" },
            { "JAP_historical_plan", "JAP_fascism" },
            { "JAP_alternate_plan_strike_north", "JAP_neutrality" },
            { "ITA_historical_plan", "ITA_fascism" },
            { "HUN_historical", "HUN_neutrality" },
            { "HUN_alternate_COM", "HUN_communism" },
            { "HUN_alternate_FAS", "HUN_fascism" },
            { "HUN_alternate_DEM", "HUN_democratic" },
            { "HUN_alternate_AH", "HUN_EMPIRE_neutrality" },
            { "HOL_historical_plan", "HOL_democratic" },
            { "HOL_alternate_monarchist_plan", "HOL_neutrality" },
            { "HOL_lead_the_minor_democracies_democratic_plan", "HOL_benelux_unified_democratic" },
            { "GRE_historical_plan", "GRE_neutrality" },
            { "GRE_alternate_fascist_plan", "byz_unified_fascism" },
            { "GRE_alternate_monarchist_plan", "MAE_fascism" },
            { "GRE_monarchist_plan", "GRE_dem_monarchy_neutrality" },
            { "German_alternate_kaiser", "GER_neutrality" },
            { "German_historical", "GER_fascism" },
            { "FRA_historical_plan", "FRA_democratic" },
            { "FRA_alternate_plan_monarchist_bonaparte", "FRA_THIRD_EMPIRE_neutrality" },
            { "FRA_alternate_plan_monarchist_legitimate", "FRA_neutrality" },
            { "FRA_alternate_plan_monarchist_orleans", "FRA_neutrality" },
            { "FRA_alternate_plan_little_entente", "FRA_democratic" },
            { "EST_baltic_entente", "bal_unified_democratic" },
            { "EST_historical", "EST_neutrality" },
            { "ENG_historical_plan", "ENG_democratic" },
            { "ENG_alternate_unaligned_plan", "ENG_neutrality" },
            { "CZE_historical_strategy_plan", "CZE_democratic" },
            { "CZE_alternate_strategy_plan_entente", "CZE_democratic" },
            { "CHI_nationalist_historical_plan", "CHI_neutrality" },
            { "CAN_historical", "CAN_ALY" },
            { "BUL_historical_plan", "BUL_neutrality" },
            { "BUL_democratic_socialist", "UBF_united_balkan_federation_democratic" },
            { "BUL_the_return_of_ferdinand_plan", "BUL_neutrality" },
            { "BUL_communsit_alternate_plan", "UBF_united_balkan_federation_communism" },
            { "BUL_communsit_plan", "BUL_communism" },
            { "LAT_historical", "LAT_communism" },
            { "AST_historical", "AST_democratic" }

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

            foreach(var id in CountryParser.StrategyPlanNameList)
            {
                string tag = id.Substring(0, 3).ToUpper();
                string guess = "";
                bool b1 = id.Contains("communism");
                bool b2 = id.Contains("communist");

                if (id.Contains("fascist"))
                    guess = $"{tag}_fascism";
                else if (id.Contains("democratic"))
                    guess = $"{tag}_democratic";
                else if (id.Contains("communism") || id.Contains("communist"))
                    guess = $"{tag}_communism";
                else if (id.Contains("neutrality"))
                    guess = $"{tag}_neutrality";

                if(!CountryParser.DisablePlanList.Contains(id))
                {
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

            DataService.WriteFlagMatches(matches);
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

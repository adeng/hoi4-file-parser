using HoI4Parser.Parsers;
using HoI4Parser.Services;
using System;

namespace HoI4Parser
{
    class Program
    {
        const string filepath = @"C:\Program Files (x86)\Steam\steamapps\common\Hearts of Iron IV";

        static void Main(string[] args)
        {
            // Initialize DB
            DataService.InitializeSQL();

            UnitsParser.LoadEquipment(filepath + @"\common\units\equipment");
            UnitsParser.LoadRegiments(filepath + @"\common\units");
            UnitsParser.LoadTerrain(filepath + @"\common\terrain");
            CountryParser.LoadCountryTags(filepath + @"\common\country_tags");
            LocalizationParser.LoadLocalizationFolder(filepath + @"\localisation\english");
            CountryParser.LoadAIStrategies(filepath + @"\common\ai_strategy_plans");
            ImageParser.LoadFlags(filepath + @"\gfx\flags", "full");
            ImageParser.LoadFlags(filepath + @"\gfx\flags\medium", "medium");
            ImageParser.LoadFlags(filepath + @"\gfx\flags\small", "small");

            // Finish
            Console.WriteLine("All files parsed and loaded");

            // Write output
            OutputService.GetTransformedRegiments();
            OutputService.GetRegiments();
            OutputService.GetEquipment();
            OutputService.GetTerrainModifiers();
            OutputService.GetCountries();

            // Finish
            Console.WriteLine("All output JSONs transformed and generated");
        }
    }
}

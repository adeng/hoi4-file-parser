﻿using HoI4Parser.Parsers;
using System;

namespace HoI4Parser
{
    class Program
    {
        const string filepath = @"C:\Program Files (x86)\Steam\steamapps\common\Hearts of Iron IV";

        static void Main(string[] args)
        {
            UnitsParser.LoadEquipment(filepath + @"\common\units\equipment");
            UnitsParser.LoadRegiments(filepath + @"\common\units");
            CountryParser.LoadCountryTags(filepath + @"\common\country_tags");
        }
    }
}

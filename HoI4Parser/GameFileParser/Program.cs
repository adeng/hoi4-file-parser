using System;

namespace HoI4Parser
{
    class Program
    {
        const string filepath = @"C:\Program Files (x86)\Steam\steamapps\common\Hearts of Iron IV";

        private static void ParseCountries()
        {

        }

        private static void ParseFocuses()
        {

        }

        static void Main(string[] args)
        {
            EquipmentParser.LoadEquipment(filepath + @"\common\units\equipment");
        }
    }
}

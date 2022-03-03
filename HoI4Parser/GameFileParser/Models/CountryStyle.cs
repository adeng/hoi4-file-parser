using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class CountryStyle : IParadoxRead
    {
        public string GraphicalCulture { get; set; }
        public string GraphicalCulture2D { get; set; }
        public IList<int> ColorCodes { get; set; }

        public CountryStyle()
        {
            ColorCodes = new List<int>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch(token)
            {
                case "graphical_culture":
                    GraphicalCulture = parser.ReadString();
                    break;

                case "graphical_culture_2d":
                    GraphicalCulture2D = parser.ReadString();
                    break;

                case "color":
                    ColorCodes = parser.ReadIntList();
                    break;
            }
        }
    }
}

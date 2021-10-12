using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameFileParser.Models
{
    class EquipmentFamily : IParadoxRead
    {
        public IList<LandEquipment> EquipmentList { get; set; }

        public EquipmentFamily()
        {
            EquipmentList = new List<LandEquipment>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            EquipmentList = parser.Parse(new LandEquipmentShell()).StatisticsList;

            // Iterate through and fetch archetype information

        }
    }
}

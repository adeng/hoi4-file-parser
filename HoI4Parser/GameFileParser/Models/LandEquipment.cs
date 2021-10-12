using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameFileParser.Models
{
    class LandEquipment : IParadoxRead
    {
        public string NameId { get; set; }
        public LandEquipmentStatistics Statistics { get; set; }


        public void TokenCallback(ParadoxParser parser, string token)
        {
            NameId = token;
            Statistics = parser.Parse(new LandEquipmentStatistics());
        }
    }
}

using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameFileParser.Models
{
    class LandEquipmentShell : IParadoxRead
    {
        public string NameId { get; set; }
        public IList<LandEquipment> StatisticsList { get; set; }

        public LandEquipmentShell()
        {
            StatisticsList = new List<LandEquipment>();
        }


        public void TokenCallback(ParadoxParser parser, string token)
        {
            var id = token;
            StatisticsList.Add(parser.Parse(new LandEquipment()));
            StatisticsList[StatisticsList.Count - 1].ID = id;
        }
    }
}

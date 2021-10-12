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
            Dictionary<string, LandEquipment> temp = new Dictionary<string, LandEquipment>();
            for(int i = EquipmentList.Count - 1; i >= 0; i--)
            {
                temp.Add(EquipmentList[i].ID, EquipmentList[i]);
            }

            // Iterate over and correct information
            for(int j = EquipmentList.Count - 1; j >= 0; j--)
            {
                if(!EquipmentList[j].IsArchetype)
                {
                    LandEquipment archetype = temp[EquipmentList[j].Archetype];
                    EquipmentList[j].AirAttack = archetype.AirAttack;
                    EquipmentList[j].APAttack = archetype.APAttack;
                    EquipmentList[j].HardAttack = archetype.HardAttack;
                    EquipmentList[j].SoftAttack = archetype.SoftAttack;
                    EquipmentList[j].ArmorValue = archetype.ArmorValue;
                    EquipmentList[j].Hardness = archetype.Hardness;
                    EquipmentList[j].Breakthrough = archetype.Breakthrough;
                    EquipmentList[j].Defense = archetype.Defense;
                    EquipmentList[j].MaximumSpeed = archetype.MaximumSpeed;
                    EquipmentList[j].Reliability = archetype.Reliability;
                }
            }
        }
    }
}

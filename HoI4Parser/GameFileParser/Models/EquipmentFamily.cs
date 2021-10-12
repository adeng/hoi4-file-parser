using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class EquipmentFamily : IParadoxRead
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
                    EquipmentList[j].AirAttack = EquipmentList[j].AirAttack == 0 ? archetype.AirAttack : EquipmentList[j].AirAttack;
                    EquipmentList[j].APAttack = EquipmentList[j].APAttack == 0 ? archetype.APAttack : EquipmentList[j].APAttack;
                    EquipmentList[j].HardAttack = EquipmentList[j].HardAttack == 0 ? archetype.HardAttack : EquipmentList[j].HardAttack;
                    EquipmentList[j].SoftAttack = EquipmentList[j].SoftAttack == 0 ? archetype.SoftAttack : EquipmentList[j].SoftAttack;
                    EquipmentList[j].ArmorValue = EquipmentList[j].ArmorValue == 0 ? archetype.ArmorValue : EquipmentList[j].ArmorValue;
                    EquipmentList[j].Hardness = EquipmentList[j].Hardness == 0 ? archetype.Hardness : EquipmentList[j].Hardness;
                    EquipmentList[j].Breakthrough = EquipmentList[j].Breakthrough == 0 ? archetype.Breakthrough : EquipmentList[j].Breakthrough;
                    EquipmentList[j].Defense = EquipmentList[j].Defense == 0 ? archetype.Defense : EquipmentList[j].Defense;
                    EquipmentList[j].MaximumSpeed = EquipmentList[j].MaximumSpeed == 0 ? archetype.MaximumSpeed : EquipmentList[j].MaximumSpeed;
                    EquipmentList[j].Reliability = EquipmentList[j].Reliability == 0 ? archetype.Reliability : EquipmentList[j].Reliability;
                    EquipmentList[j].BuildCostIC = EquipmentList[j].BuildCostIC == 0 ? archetype.BuildCostIC : EquipmentList[j].BuildCostIC;
                    
                    if(EquipmentList[j].Type.Count == 0)
                        EquipmentList[j].Type = archetype.Type;
                }
                //else
                //{
                //    // Remove archetypes since they aren't real
                //    EquipmentList.Remove(EquipmentList[j]);
                //}
            }
        }
    }
}

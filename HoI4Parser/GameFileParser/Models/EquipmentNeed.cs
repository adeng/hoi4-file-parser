using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class EquipmentNeed : IParadoxRead
    {
        public int Number { get; set; }
        public string EquipmentID { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            EquipmentID = token;
            Number = parser.ReadInt32();
        }
    }
}

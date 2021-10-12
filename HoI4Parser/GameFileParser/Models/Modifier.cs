using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class Modifier : IParadoxRead
    {
        public string ModifierType { get; set; }
        public double Attack { get; set; }
        public double Movement { get; set; }
        public double Defense { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch(token)
            {
                case "attack":
                    Attack = parser.ReadDouble();
                    break;
                case "movement":
                    Movement = parser.ReadDouble();
                    break;
                case "defense":
                    Defense = parser.ReadDouble();
                    break;
            }
        }
    }
}

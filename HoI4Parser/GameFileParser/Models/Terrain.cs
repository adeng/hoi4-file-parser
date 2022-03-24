using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class Terrain : IParadoxRead
    {
        public string ID { get; set; }
        public IList<int> ColorCodes { get; set; }
        public bool IsWater { get; set; }
        public int CombatWidth { get; set; }
        public int CombatSupportWidth { get; set; }
        public double AttackModifier { get; set; }
        public double DefenseModifier { get; set; }
        public bool NavalTerrain { get; set; }
        public IList<Tuple<string, Modifier>> Modifiers { get; set; }

        public Terrain()
        {
            Modifiers = new List<Tuple<string, Modifier>>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "naval_terrain":
                    NavalTerrain = parser.ReadString() == "yes";
                    break;
                case "units":
                    if (!NavalTerrain) // List modifier for all if not naval terrain; otherwise, this is parsed as part of the naval terrain
                        Modifiers.Add(new Tuple<string, Modifier>("all", parser.Parse(new Modifier())));
                    break;
                case "battle_cruiser":
                case "battleship":
                case "heavy_cruiser":
                case "carrier":
                case "destroyer":
                case "light_cruiser":
                case "submarine":
                    Modifiers.Add(new Tuple<string, Modifier>(token, parser.Parse(new Modifier())));
                    break;
                case "color":
                    ColorCodes = parser.ReadIntList();
                    break;
                case "combat_width":
                    CombatWidth = parser.ReadInt32();
                    break;
                case "combat_support_width":
                    CombatSupportWidth = parser.ReadInt32();
                    break;
                case "is_water":
                    IsWater = parser.ReadString() == "yes";
                    break;

            }
        }
    }
}

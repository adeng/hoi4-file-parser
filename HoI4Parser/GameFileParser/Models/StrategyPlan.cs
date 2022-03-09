using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class StrategyPlan : IParadoxRead
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ideology { get; set; }
        public string Tag { get; set; }
        public IList<string> NationalFocuses { get; set; }
        public bool Enabled { get; set; }

        public StrategyPlan()
        {
            NationalFocuses = new List<string>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch(token)
            {
                case "name":
                    Name = parser.ReadString();
                    break;
                case "description":
                    Description = parser.ReadString();
                    break;
                case "ai_national_focuses":
                    NationalFocuses = parser.ReadStringList();
                    break;
                case "option":
                    Ideology = parser.ReadString();
                    break;
                case "original_tag":
                    Tag = parser.ReadString();
                    break;
            }
        }
    }
}

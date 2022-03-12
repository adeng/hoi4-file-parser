using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class Regiment : IParadoxRead
    {
        public string ID { get; set; }
        public int Priority { get; set; }
        public bool Active { get; set; }
        public bool AffectsSpeed { get; set; }
        public bool CanBeParachuted { get; set; }
        public IList<string> Type { get; set; }
        public IList<string> Categories { get; set; }
        public double Entrenchment { get; set; }
        public double Recon { get; set; }
        public string Group { get; set; }
        public int CombatWidth { get; set; }
        public int Manpower { get; set; }
        public double MaxStrength { get; set; }
        public double MaxOrganization { get; set; }
        public double DefaultMorale { get; set; }
        public double Weight { get; set; }
        public double SupplyConsumption { get; set; }
        public double Suppression { get; set; }
        public double SuppressionFactor { get; set; }
        public double Initiative { get; set; }
        public string SameSupportType { get; set; }
        public IList<EquipmentNeed> Needs { get; set; }
        public IList<Modifier> Modifiers { get; set; }

        public Regiment()
        {
            Type = new List<string>();
            Needs = new List<EquipmentNeed>();
            Modifiers = new List<Modifier>();
            Categories = new List<string>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch(token)
            {
                case "priority":
                    Priority = parser.ReadInt32();
                    break;
                case "active":
                    Active = parser.ReadString() == "yes";
                    break;
                case "type":
                    Type = parser.ReadStringList();
                    break;
                case "group":
                    Group = parser.ReadString();
                    break;
                case "categories":
                    Categories = parser.ReadStringList();
                    break;
                case "combat_width":
                    CombatWidth = parser.ReadInt32();
                    break;
                case "need":
                    var dict = parser.ReadDictionary<string, int>(p => p.ReadString(), p => p.ReadInt32());

                    foreach (var item in dict)
                    {
                        Needs.Add(new EquipmentNeed(item.Key, item.Value));
                    }
                    break;
                case "manpower":
                    Manpower = parser.ReadInt32();
                    break;
                case "entrenchment":
                    Entrenchment = parser.ReadDouble();
                    break;
                case "recon":
                    Recon = parser.ReadDouble();
                    break;
                case "max_organisation":
                    MaxOrganization = parser.ReadDouble();
                    break;
                case "default_morale":
                    DefaultMorale = parser.ReadDouble();
                    break;
                case "max_strength":
                    MaxStrength = parser.ReadDouble();
                    break;
                case "weight":
                    Weight = parser.ReadDouble();
                    break;
                case "supply_consumption":
                    SupplyConsumption = parser.ReadDouble();
                    break;
                case "suppression":
                    Suppression = parser.ReadDouble();
                    break;
                case "suppression_factor":
                    SuppressionFactor = parser.ReadDouble();
                    break;
                case "initiative":
                    Initiative = parser.ReadDouble();
                    break;
                case "can_be_parachuted":
                    CanBeParachuted = parser.ReadString() == "yes";
                    break;
                case "same_support_type":
                    SameSupportType = parser.ReadString();
                    break;
                case "forest":
                case "hills":
                case "mountain":
                case "jungle":
                case "marsh":
                case "fort":
                case "urban":
                case "plains":
                case "desert":
                case "river":
                case "amphibious":
                    var type = token;
                    Modifiers.Add(parser.Parse(new Modifier()));
                    Modifiers[Modifiers.Count - 1].ModifierType = type;
                    break;
            }
        }
    }
}

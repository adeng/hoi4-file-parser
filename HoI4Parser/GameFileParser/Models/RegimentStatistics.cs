using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class RegimentStatistics
    {
        public string ID { get; set; }
        public double HP { get; set; }
        public double Organization { get; set; }
        public long Year { get; set; }
        public double SoftAttack { get; set; }
        public double HardAttack { get; set; }
        public double Breakthrough { get; set; }
        public double Defense { get; set; }
        public double Piercing { get; set; }
        public double AirAttack { get; set; }
        public double Armor { get; set; }
        public double Hardness { get; set; }
        public long CombatWidth { get; set; }
        public double Cost { get; set; }

        public RegimentStatistics(string id, double hp, double org, long year, double soft, double hard, double breakthrough, double defense, double piercing, double air, double armor, double hardness, long width, double cost)
        {
            ID = id;
            HP = hp;
            Organization = org;
            Year = Year;
            SoftAttack = soft;
            HardAttack = hard;
            Breakthrough = breakthrough;
            Defense = defense;
            Piercing = piercing;
            AirAttack = air;
            Armor = armor;
            Hardness = hardness;
            CombatWidth = width;
            Cost = cost;
        }
    }
}

﻿using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameFileParser.Models
{
    class LandEquipmentStatistics : IParadoxRead
    {
        public int Year { get; set; }
        public bool IsArchetype { get; set; }
        public string Archetype { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public double Reliability { get; set; }
        public double MaximumSpeed { get; set; }
        public double Defense { get; set; }
        public double Breakthrough { get; set; }
        public double Hardness { get; set; }
        public double ArmorValue { get; set; }
        public double SoftAttack { get; set; }
        public double HardAttack { get; set; }
        public double APAttack { get; set; }
        public double AirAttack { get; set; }
        public double FuelConsumption { get; set; }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "year": 
                    Year = parser.ReadInt32();
                    break;
                case "is_archetype":
                    IsArchetype = parser.ReadString() == "yes";
                    break;
                case "archetype":
                    Archetype = parser.ReadString();
                    break;
                case "type":
                    Type = parser.ReadString();
                    break;
                case "active":
                    Active = parser.ReadString() == "yes";
                    break;
                case "reliability":
                    Reliability = parser.ReadDouble();
                    break;
                case "maximum_speed":
                    MaximumSpeed = parser.ReadDouble();
                    break;
                case "defense":
                    Defense = parser.ReadDouble();
                    break;
                case "breakthrough":
                    Breakthrough = parser.ReadDouble();
                    break;
                case "hardness":
                    Hardness = parser.ReadDouble();
                    break;
                case "armor_value":
                    ArmorValue = parser.ReadDouble();
                    break;
                case "soft_attack":
                    SoftAttack = parser.ReadDouble();
                    break;
                case "hard_attack":
                    HardAttack = parser.ReadDouble();
                    break;
                case "fuel_consumption":
                    FuelConsumption = parser.ReadDouble();
                    break;

            }
        }
    }
}

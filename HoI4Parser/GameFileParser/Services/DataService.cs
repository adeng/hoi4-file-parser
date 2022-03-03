using HoI4Parser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HoI4Parser.Services
{
    public static class DataService
    {
        /// <summary>
        /// Master dataset for application
        /// </summary>
        public static DataSet HoI4Set = new DataSet();

        public static DataTable EquipmentTable = CreateDataTable("EquipmentTable", new List<Tuple<string, string>>
        {
            new Tuple<string, string>("ID", "System.String"),
            new Tuple<string, string>("YEAR", "System.Int32"),
            new Tuple<string, string>("IS_ARCHETYPE", "System.Boolean"),
            new Tuple<string, string>("ARCHETYPE", "System.String"),
            new Tuple<string, string>("TYPE", "System.String"),
            new Tuple<string, string>("ACTIVE", "System.Boolean"),
            new Tuple<string, string>("RELIABILITY", "System.Double"),
            new Tuple<string, string>("MAXIMUM_SPEED", "System.Double"),
            new Tuple<string, string>("DEFENSE", "System.Double"),
            new Tuple<string, string>("BREAKTHROUGH", "System.Double"),
            new Tuple<string, string>("HARDNESS", "System.Double"),
            new Tuple<string, string>("ARMOR_VALUE", "System.Double"),
            new Tuple<string, string>("SOFT_ATTACK", "System.Double"),
            new Tuple<string, string>("HARD_ATTACK", "System.Double"),
            new Tuple<string, string>("AP_ATTACK", "System.Double"),
            new Tuple<string, string>("AIR_ATTACK", "System.Double"),
            new Tuple<string, string>("FUEL_CONSUMPTION", "System.Double"),
            new Tuple<string, string>("BUILD_COST_IC", "System.Double")
        });

        public static DataTable RegimentTable = CreateDataTable("RegimentTable", new List<Tuple<string, string>>
        {
            new Tuple<string, string>("ID", "System.String"),
            new Tuple<string, string>("PRIORITY", "System.Int32"),
            new Tuple<string, string>("ACTIVE", "System.Boolean"),
            new Tuple<string, string>("AFFECTS_SPEED", "System.Boolean"),
            new Tuple<string, string>("CAN_BE_PARACHUTED", "System.Boolean"),
            new Tuple<string, string>("TYPE", "System.String"),
            new Tuple<string, string>("ENTRENCHMENT", "System.Double"),
            new Tuple<string, string>("RECON", "System.Double"),
            new Tuple<string, string>("GROUP", "System.String"),
            new Tuple<string, string>("COMBAT_WIDTH", "System.Int32"),
            new Tuple<string, string>("MANPOWER", "System.Int32"),
            new Tuple<string, string>("MAX_STRENGTH", "System.Double"),
            new Tuple<string, string>("MAX_ORGANIZATION", "System.Double"),
            new Tuple<string, string>("DEFAULT_MORALE", "System.Double"),
            new Tuple<string, string>("WEIGHT", "System.Double"),
            new Tuple<string, string>("SUPPLY_CONSUMPTION", "System.Double"),
            new Tuple<string, string>("SUPPRESSION", "System.Double"),
            new Tuple<string, string>("SUPPRESSION_FACTOR", "System.Double"),
            new Tuple<string, string>("INITIATIVE", "System.Double"),
            new Tuple<string, string>("SAME_SUPPORT_TYPE", "System.String")
        });

        public static DataTable RegimentModifiersTable = CreateDataTable("RegimentModifiersTable", new List<Tuple<string,string>>
        {
            new Tuple<string, string>("ID", "System.String"),
            new Tuple<string, string>("MODIFIER_TYPE", "System.String"),
            new Tuple<string, string>("ATTACK", "System.Double"),
            new Tuple<string, string>("MOVEMENT", "System.Double"),
            new Tuple<string, string>("DEFENSE", "System.Double")
        });

        public static DataTable RegimentCategoriesTable = CreateDataTable("RegimentCategoriesTable", new List<Tuple<string, string>>
        {
            new Tuple<string, string>("ID", "System.String"),
            new Tuple<string, string>("CATEGORY", "System.String")
        });

        public static DataTable RegimentNeedsTable = CreateDataTable("RegimentNeedsTable", new List<Tuple<string, string>>
        {
            new Tuple<string, string>("ID", "System.String"),
            new Tuple<string, string>("EQUIPMENT_ID", "System.String"),
            new Tuple<string, string>("NUMBER", "System.Int32")
        });

        public static Dictionary<string, LandEquipment> EquipmentDictionary = new Dictionary<string, LandEquipment>();
        public static Dictionary<string, Regiment> RegimentDictionary = new Dictionary<string, Regiment>();

        /// <summary>
        /// Constructs a DataTable (without data) with the given columns and types. If types are not defined,
        /// this will default them to type String.
        /// </summary>
        /// <param name="name">Name of the DataTable.</param>
        /// <param name="columns">A list of columns to add to the DataTable.</param>
        /// <param name="types">An optional list of the types for each column.</param>
        /// <returns>A DataTable containing the requested data, but without data.</returns>0
        public static DataTable CreateDataTable(string name, List<string> columns, List<string> types = null)
        {
            DataTable table = HoI4Set.Tables.Add(name);

            // For each column, create a new column in the DataTable
            for(int i = 0; i < columns.Count; i++)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = columns[i];
                col.AllowDBNull = true;

                // If the types argument was passed, please use it to specify the type
                if (types != null)
                    col.DataType = Type.GetType(types[i]);
                else
                    col.DataType = Type.GetType("System.String");
                table.Columns.Add(col);
            }

            return table;
        }

        public static DataTable CreateDataTable(string name, List<Tuple<string, string>> columns)
        {
            DataTable table = HoI4Set.Tables.Add(name);

            // For each column, create a new column in the DataTable
            for (int i = 0; i < columns.Count; i++)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = columns[i].Item1;
                col.AllowDBNull = true;
                col.DataType = Type.GetType(columns[i].Item2);
                table.Columns.Add(col);
            }

            return table;
        }

        public static void WriteLandEquipment(EquipmentFamily family)
        {
            for(int j = family.EquipmentList.Count - 1; j >= 0; j--)
            {
                for (int k = family.EquipmentList[j].Type.Count - 1; k >= 0; k--)
                {
                    // Insert into table
                    DataRow row = EquipmentTable.NewRow();
                    var equip = family.EquipmentList[j];

                    row["ID"] = equip.ID;
                    row["YEAR"] = equip.Year;
                    row["IS_ARCHETYPE"] = equip.IsArchetype;
                    row["ARCHETYPE"] = equip.Archetype;
                    row["TYPE"] = family.EquipmentList[j].Type[k];
                    row["ACTIVE"] = equip.Active;
                    row["RELIABILITY"] = equip.Reliability;
                    row["MAXIMUM_SPEED"] = equip.MaximumSpeed;
                    row["DEFENSE"] = equip.Defense;
                    row["BREAKTHROUGH"] = equip.Breakthrough;
                    row["HARDNESS"] = equip.Hardness;
                    row["ARMOR_VALUE"] = equip.ArmorValue;
                    row["SOFT_ATTACK"] = equip.SoftAttack;
                    row["HARD_ATTACK"] = equip.HardAttack;
                    row["AP_ATTACK"] = equip.APAttack;
                    row["AIR_ATTACK"] = equip.AirAttack;
                    row["FUEL_CONSUMPTION"] = equip.FuelConsumption;
                    row["BUILD_COST_IC"] = equip.BuildCostIC;

                    EquipmentTable.Rows.Add(row);
                }
            }

            EquipmentTable.AcceptChanges();
        }

        public static void WriteRegiment(RegimentFamily family)
        {
            for (int i = family.RegimentList.Count - 1; i >= 0; i--)
            {
                var regiment = family.RegimentList[i];

                // Write to RegimentTable
                for(int k = regiment.Type.Count - 1; k >= 0; k--)
                {
                    // Insert into tables
                    DataRow row = RegimentTable.NewRow();

                    row["ID"] = regiment.ID;
                    row["PRIORITY"] = regiment.Priority;
                    row["ACTIVE"] = regiment.Active;
                    row["AFFECTS_SPEED"] = regiment.AffectsSpeed;
                    row["CAN_BE_PARACHUTED"] = regiment.CanBeParachuted;
                    row["TYPE"] = regiment.Type[k];
                    row["ENTRENCHMENT"] = regiment.Entrenchment;
                    row["RECON"] = regiment.Recon;
                    row["GROUP"] = regiment.Group;
                    row["COMBAT_WIDTH"] = regiment.CombatWidth;
                    row["MANPOWER"] = regiment.Manpower;
                    row["MAX_STRENGTH"] = regiment.MaxStrength;
                    row["MAX_ORGANIZATION"] = regiment.MaxOrganization;
                    row["DEFAULT_MORALE"] = regiment.DefaultMorale;
                    row["WEIGHT"] = regiment.Weight;
                    row["SUPPLY_CONSUMPTION"] = regiment.SupplyConsumption;
                    row["SUPPRESSION"] = regiment.Suppression;
                    row["SUPPRESSION_FACTOR"] = regiment.SuppressionFactor;
                    row["INITIATIVE"] = regiment.Initiative;
                    row["SAME_SUPPORT_TYPE"] = regiment.SameSupportType;

                    RegimentTable.Rows.Add(row);
                }

                // Write to RegimentModifiers table
                for(int j = regiment.Modifiers.Count - 1; j >= 0; j--)
                {
                    DataRow row = RegimentModifiersTable.NewRow();

                    row["ID"] = regiment.ID;
                    row["MODIFIER_TYPE"] = regiment.Modifiers[j].ModifierType;
                    row["ATTACK"] = regiment.Modifiers[j].Attack;
                    row["MOVEMENT"] = regiment.Modifiers[j].Movement;
                    row["DEFENSE"] = regiment.Modifiers[j].Defense;

                    RegimentModifiersTable.Rows.Add(row);
                }

                // Write to RegimentCategories table
                for (int l = regiment.Categories.Count - 1; l >= 0; l--)
                {
                    DataRow row = RegimentCategoriesTable.NewRow();

                    row["ID"] = regiment.ID;
                    row["CATEGORY"] = regiment.Categories[l];

                    RegimentCategoriesTable.Rows.Add(row);
                }

                // Write to the RequirementNeeds table
                for (int m = regiment.Needs.Count - 1; m >= 0; m--)
                {
                    DataRow row = RegimentNeedsTable.NewRow();

                    row["ID"] = regiment.ID;
                    row["EQUIPMENT_ID"] = regiment.Needs[m].EquipmentID;
                    row["NUMBER"] = regiment.Needs[m].Number;

                    RegimentNeedsTable.Rows.Add(row);
                }
            }

            RegimentTable.AcceptChanges();
            RegimentCategoriesTable.AcceptChanges();
            RegimentModifiersTable.AcceptChanges();
            RegimentNeedsTable.AcceptChanges();
        }
    }
}

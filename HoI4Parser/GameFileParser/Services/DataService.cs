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


        public static DataTable EquipmentTable = CreateDataTable("equipment", new List<string>
            {
                "ID", "YEAR", "IS_ARCHETYPE", "ARCHETYPE", "TYPE", "ACTIVE", "RELIABILITY", "MAXIMUM_SPEED", "DEFENSE", "BREAKTHROUGH", "HARDNESS", "ARMOR_VALUE", "SOFT_ATTACK", "HARD_ATTACK", "AP_ATTACK", "AIR_ATTACK", "FUEL_CONSUMPTION", "BUILD_COST_IC"
            }, new List<string>
            {
                "System.String", "System.Int32", "System.Boolean", "System.String", "System.String", "System.Boolean", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double", "System.Double"
            });

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
    }
}

using HoI4Parser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace HoI4Parser.Services
{
    public static class DataService
    {
        private static Dictionary<string, string> TYPE_TRANSLATIONS = new Dictionary<string, string>
            {
                {
                    "System.String",
                    "TEXT"
                },
                {
                    "System.Double",
                    "REAL"
                },
                {
                    "System.Boolean",
                    "TEXT"
                },
                {
                    "System.Int32",
                    "INTEGER"
                }
            };

        private const string DATABASE_STRING = "parser_database.db";
        private const string READ_DATABASE_STRING = "Data Source=parser_database.db";

        public static Dictionary<string, LandEquipment> EquipmentDictionary = new Dictionary<string, LandEquipment>();
        public static Dictionary<string, Regiment> RegimentDictionary = new Dictionary<string, Regiment>();

        public static void InitializeSQL()
        {
            if (File.Exists(DATABASE_STRING))
                File.Delete(DATABASE_STRING);

            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                CreateSQLTable("LocalizationTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("LOCALIZATION_KEY", "System.String"),
                    new Tuple<string, string>("LOCALIZATION_VALUE", "System.String")
                }, connection);

                CreateSQLTable("CountryTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("TAG", "System.String"),
                    new Tuple<string, string>("NAME", "System.String"),
                    new Tuple<string, string>("COLOR", "System.String")
                }, connection);

                CreateSQLTable("EquipmentTable", new List<Tuple<string, string>>
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
                }, connection);

                CreateSQLTable("RegimentTable", new List<Tuple<string, string>>
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
                }, connection);

                CreateSQLTable("RegimentModifiersTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("ID", "System.String"),
                    new Tuple<string, string>("MODIFIER_TYPE", "System.String"),
                    new Tuple<string, string>("ATTACK", "System.Double"),
                    new Tuple<string, string>("MOVEMENT", "System.Double"),
                    new Tuple<string, string>("DEFENSE", "System.Double")
                }, connection);

                CreateSQLTable("RegimentCategoriesTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("ID", "System.String"),
                    new Tuple<string, string>("CATEGORY", "System.String")
                }, connection);

                CreateSQLTable("RegimentNeedsTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("ID", "System.String"),
                    new Tuple<string, string>("EQUIPMENT_ID", "System.String"),
                    new Tuple<string, string>("NUMBER", "System.Int32")
                }, connection);

                CreateSQLTable("StrategyPlanTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("ID", "System.String"),
                    new Tuple<string, string>("TAG", "System.String"),
                    new Tuple<string, string>("NAME", "System.String"),
                    new Tuple<string, string>("DESCRIPTION", "System.String"),
                    new Tuple<string, string>("IDEOLOGY", "System.String"),
                    new Tuple<string, string>("ORDER", "System.Int32"),
                    new Tuple<string, string>("NATIONAL_FOCUS", "System.String"),
                    new Tuple<string, string>("ENABLED", "System.Boolean")
                }, connection);

                CreateSQLTable("FlagImageTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("TAG", "System.String"),
                    new Tuple<string, string>("FILENAME", "System.String"),
                    new Tuple<string, string>("BITMAP", "System.String"),
                    new Tuple<string, string>("SIZE", "System.String")
                }, connection);

                CreateSQLTable("StrategyFlagMatchTable", new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("STRATEGY_PLAN_ID", "System.String"),
                    new Tuple<string, string>("FLAG_FILENAME", "System.String")
                }, connection);
            }
        }

        public static void CreateSQLTable(string name, List<Tuple<string, string>> columns, SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                string sql = $@"CREATE TABLE [{name}] (";

                for (int i = 0; i < columns.Count; i++)
                {
                    sql += $"[{columns[i].Item1}] {TYPE_TRANSLATIONS[columns[i].Item2]}";
                    sql += i == columns.Count - 1 ? "" : ",";
                }

                sql += ")";

                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        // Write Queries
        public static void WriteLandEquipment(EquipmentFamily family)
        {
            if (family.EquipmentList.Count == 0) return;

            string sql = "INSERT INTO EquipmentTable VALUES";

            for (int j = family.EquipmentList.Count - 1; j >= 0; j--)
            {
                for (int k = family.EquipmentList[j].Type.Count - 1; k >= 0; k--)
                {
                    var equip = family.EquipmentList[j];

                    sql += $"('{equip.ID}',{equip.Year},'{equip.IsArchetype}','{equip.Archetype}','{family.EquipmentList[j].Type[k]}','{equip.Active}'," +
                        $"{equip.Reliability},{equip.MaximumSpeed},{equip.Defense},{equip.Breakthrough},{equip.Hardness},{equip.ArmorValue}," +
                        $"{equip.SoftAttack},{equip.HardAttack},{equip.APAttack},{equip.AirAttack},{equip.FuelConsumption},{equip.BuildCostIC})";

                    if (!(j == 0 && k == 0))
                        sql += ",";
                }
            }

            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void WriteLocalizations(List<Tuple<string, string>> localizations)
        {
            string sql = "INSERT INTO LocalizationTable VALUES";


            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                for (int i = localizations.Count - 1, j = 1; i >= 0; i--, j++)
                {
                    sql += $"('{localizations[i].Item1.Replace("\'", "\'\'")}','{localizations[i].Item2.Replace("\'", "\'\'")}'),";

                    if (j % 10000 == 0)
                    {
                        sql = sql.TrimEnd(',');

                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        sql = "INSERT INTO LocalizationTable VALUES";
                    }
                }

                // Flush out what's left over 
                sql = sql.TrimEnd(',');

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void WriteCountries(List<Country> countries)
        {
            string sql = "INSERT INTO CountryTable VALUES";

            for (int i = countries.Count - 1; i >= 0; i--)
            {
                sql += $"('{countries[i].Tag}','{countries[i].Name}','{countries[i].Color}')";

                if (i > 0)
                    sql += ",";
            }

            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void WriteFlagMatches(List<Tuple<string, string>> matches)
        {
            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                string sql = "INSERT INTO StrategyFlagMatchTable VALUES";
                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    var match = matches[i];

                    sql += $"('{match.Item1}','{match.Item2}'),";
                }

                sql = sql.TrimEnd(',');

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void WriteFlagImages(List<FlagImage> images)
        {
            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                string sql = "INSERT INTO FlagImageTable VALUES";
                for (int i = images.Count - 1; i >= 0; i--)
                {
                    FlagImage image = images[i];

                    sql += $"('{image.Tag}','{image.FileName}','{image.Bitmap}','{image.Size}'),";
                }

                sql = sql.TrimEnd(',');

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void WriteStrategy(StrategyPlanShell plans)
        {
            if (plans.StrategyPlanList.Count == 0)
                return;

            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                for (int i = plans.StrategyPlanList.Count - 1; i >= 0; i--)
                {
                    StrategyPlan plan = plans.StrategyPlanList[i];
                    if (plan.NationalFocuses.Count == 0)
                        continue;

                    string sql = "INSERT INTO StrategyPlanTable VALUES";

                    for (int j = 0; j < plan.NationalFocuses.Count; j++)
                    {
                        string description = plan.Description;
                        if (description != null)
                            description = description.Replace("'", "''");
                        sql += $"('{plan.ID}','{plan.Tag}','{plan.Name.Replace("'", "''")}','{description}','{plan.Ideology}',{j + 1},'{plan.NationalFocuses[j]}','{plan.Enabled}'),";
                    }

                    sql = sql.TrimEnd(',');

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                        sql = "INSERT INTO StrategyPlanTable VALUES";
                    }
                }
            }
        }

        public static void WriteRegiment(RegimentFamily family)
        {
            if (family.RegimentList.Count == 0)
                return;

            bool[] hasData = { false, false, false, false };

            string sqlMain = "INSERT INTO RegimentTable VALUES";
            string sqlModifiers = "INSERT INTO RegimentModifiersTable VALUES";
            string sqlCategories = "INSERT INTO RegimentCategoriesTable VALUES";
            string sqlNeeds = "INSERT INTO RegimentNeedsTable VALUES";

            for (int i = family.RegimentList.Count - 1; i >= 0; i--)
            {
                var regiment = family.RegimentList[i];

                // Write to RegimentTable
                if (regiment.Type.Count > 0)
                {
                    for (int k = regiment.Type.Count - 1; k >= 0; k--)
                    {
                        sqlMain += $"('{regiment.ID}',{regiment.Priority},'{regiment.Active}','{regiment.AffectsSpeed}','{regiment.CanBeParachuted}'," +
                            $"'{regiment.Type[k]}',{regiment.Entrenchment},{regiment.Recon},'{regiment.Group}',{regiment.CombatWidth},{regiment.Manpower}," +
                            $"{regiment.MaxStrength},{regiment.MaxOrganization},{regiment.DefaultMorale},{regiment.Weight},{regiment.SupplyConsumption}," +
                            $"{regiment.Suppression},{regiment.SuppressionFactor},{regiment.Initiative},'{regiment.SameSupportType}'),";
                    }

                    hasData[0] = true;
                }

                // Write to RegimentModifiers table
                if (regiment.Modifiers.Count > 0)
                {
                    for (int j = regiment.Modifiers.Count - 1; j >= 0; j--)
                    {
                        sqlModifiers += $"('{regiment.ID}','{regiment.Modifiers[j].ModifierType}',{regiment.Modifiers[j].Attack},{regiment.Modifiers[j].Movement}," +
                            $"{regiment.Modifiers[j].Defense}),";
                    }

                    hasData[1] = true;
                }

                // Write to RegimentCategories table
                if (regiment.Categories.Count > 0)
                {
                    for (int l = regiment.Categories.Count - 1; l >= 0; l--)
                    {
                        sqlCategories += $"('{regiment.ID}','{regiment.Categories[l]}'),";
                    }

                    hasData[2] = true;
                }

                // Write to the RequirementNeeds table
                if (regiment.Needs.Count > 0)
                {
                    for (int m = regiment.Needs.Count - 1; m >= 0; m--)
                    {
                        sqlNeeds += $"('{regiment.ID}','{regiment.Needs[m].EquipmentID}',{regiment.Needs[m].Number}),";
                    }

                    hasData[3] = true;
                }
            }

            sqlMain = sqlMain.TrimEnd(',');
            sqlNeeds = sqlNeeds.TrimEnd(',');
            sqlModifiers = sqlModifiers.TrimEnd(',');
            sqlCategories = sqlCategories.TrimEnd(',');

            using (var connection = new SQLiteConnection($"Data Source={DATABASE_STRING}"))
            {
                connection.Open();

                if (hasData[0] == true)
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sqlMain;
                        command.ExecuteNonQuery();
                    }
                }

                if (hasData[1] == true)
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sqlModifiers;
                        command.ExecuteNonQuery();
                    }
                }

                if (hasData[2] == true)
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sqlCategories;
                        command.ExecuteNonQuery();
                    }
                }

                if (hasData[3] == true)
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sqlNeeds;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static DataTable GetRegimentNeeds()
        {
            using (var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                connection.Open();
                string sql = $"SELECT ID, EQUIPMENT_ID FROM RegimentNeedsTable ORDER BY ID, EQUIPMENT_ID ASC";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        return table;
                    }
                }
            }
        }

        public static DataTable GetEquipment()
        {
            using(var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                string sql = @"SELECT DISTINCT ARCHETYPE AS ARCHETYPE_ID, LTE.LOCALIZATION_VALUE AS ARCHETYPE_NAME, ID AS EQUIPMENT_ID, LT.LOCALIZATION_VALUE AS EQUIPMENT_NAME, YEAR, RELIABILITY, MAXIMUM_SPEED, DEFENSE, BREAKTHROUGH, SOFT_ATTACK, HARD_ATTACK, AP_ATTACK AS PIERCING, AIR_ATTACK, HARDNESS, FUEL_CONSUMPTION, BUILD_COST_IC AS COST, ARMOR_VALUE AS ARMOR
                    FROM EquipmentTable AS ET
                    LEFT JOIN LocalizationTable AS LT ON (ET.ID = LT.LOCALIZATION_KEY)
                    LEFT JOIN LocalizationTable AS LTE ON (ET.ARCHETYPE = LTE.LOCALIZATION_KEY)
                    WHERE IS_ARCHETYPE = 'False'
                    ORDER BY ARCHETYPE_ID, EQUIPMENT_ID";

                connection.Open();

                using(var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
        }

        public static DataTable GetTerrainModifiers()
        {
            using(var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                string sql = @"SELECT MODIFIER_TYPE AS TERRAIN, ID AS REGIMENT_ID, ATTACK, DEFENSE, MOVEMENT
                        FROM RegimentModifiersTable
                        ORDER BY TERRAIN, ID ASC";

                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    using(SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
        }

        public static DataTable GetRegiments()
        {
            using(var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                string sql = @"SELECT DISTINCT RT.ID AS REGIMENT_ID, LT.LOCALIZATION_VALUE AS REGIMENT_NAME, RT.MAX_STRENGTH AS HP, RT.MAX_ORGANIZATION AS ORGANIZATION, RT.PRIORITY, RNT.EQUIPMENT_ID AS ARCHETYPE_ID, LTE.LOCALIZATION_VALUE AS ARCHETYPE_NAME, RNT.NUMBER, RT.TYPE, RT.SAME_SUPPORT_TYPE, RT.COMBAT_WIDTH AS WIDTH
                    FROM RegimentTable RT
                    LEFT JOIN RegimentNeedsTable RNT ON (RT.ID = RNT.ID)
                    LEFT JOIN LocalizationTable LT ON (LT.LOCALIZATION_KEY = RT.ID)
                    LEFT JOIN LocalizationTable LTE ON (LTE.LOCALIZATION_KEY = RNT.EQUIPMENT_ID)
                    ORDER BY REGIMENT_ID, ARCHETYPE_ID";

                connection.Open();

                using(var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
        }

        public static DataTable GetEarliestEquipmentYears()
        {
            using (var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                connection.Open();
                string sql = @"SELECT REGIMENT_ID, MAX(YEAR) AS MAX_YEAR FROM (
	                    SELECT RNT.ID AS REGIMENT_ID, ET.ARCHETYPE, MIN(ET.YEAR) AS YEAR
	                    FROM RegimentNeedsTable RNT
	                    LEFT JOIN EquipmentTable ET ON (ET.ARCHETYPE = RNT.EQUIPMENT_ID)
	                    WHERE ET.IS_ARCHETYPE = 'False'
	                    GROUP BY REGIMENT_ID, ET.ARCHETYPE
	                    HAVING MIN(ET.YEAR) NOT NULL)
                    GROUP BY REGIMENT_ID";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        return table;
                    }
                }
            }
        }

        public static DataTable GetCalculatedRegiments(string filter = null)
        {
            using (var connection = new SQLiteConnection(READ_DATABASE_STRING))
            {
                string sql = $@"SELECT DISTINCT
	                RT.ID AS REGIMENT_ID, 
	                LT.LOCALIZATION_VALUE AS REGIMENT_NAME, 
	                RT.PRIORITY AS REGIMENT_PRIORITY, 
	                ET.ID AS EQUIPMENT_ID,
	                LTE.LOCALIZATION_VALUE AS EQUIPMENT_NAME,
	                RT.MAX_STRENGTH AS HP, 
	                RT.MAX_ORGANIZATION AS ORGANIZATION,
	                ET.YEAR AS YEAR,
	                ET.SOFT_ATTACK,
	                ET.HARD_ATTACK,
	                ET.BREAKTHROUGH,
	                ET.DEFENSE,
	                ET.AP_ATTACK AS PIERCING,
	                ET.AIR_ATTACK,
	                ET.ARMOR_VALUE AS ARMOR,
	                ET.HARDNESS,
	                RT.COMBAT_WIDTH AS COMBAT_WIDTH,
	                ET.BUILD_COST_IC * RNT.NUMBER AS COST,
	                ET.ARCHETYPE
                FROM RegimentTable AS RT
	                LEFT JOIN RegimentCategoriesTable AS RCT ON (RT.ID = RCT.ID)
	                LEFT JOIN LocalizationTable AS LT ON (RT.ID = LT.LOCALIZATION_KEY)
	                LEFT JOIN EquipmentTable AS ET ON (RNT.EQUIPMENT_ID = ET.ARCHETYPE)
	                LEFT JOIN LocalizationTable AS LTE ON (ET.ID = LTE.LOCALIZATION_KEY)
	                LEFT JOIN RegimentNeedsTable AS RNT ON (RT.ID = RNT.ID)
                WHERE ET.IS_ARCHETYPE = 'False'
                {(filter != null ? " AND RCT.CATEGORY = @category " : "")}
                ORDER BY REGIMENT_ID, YEAR ASC";

                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = sql;

                    if (filter != null)
                        command.Parameters.Add("@category", DbType.String).Value = filter;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
        }
    }
}

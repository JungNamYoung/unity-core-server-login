using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DotnetCoreServer.Models
{
    public interface IUpgradeDao{
        List<UpgradeData> GetUpgradeInfo();
        UpgradeData GetUpgradeInfo(string UpgradeType, int UpgradeLevel);
        
    }

    public class UpgradeDao : IUpgradeDao
    {
        public IDB _IDB {get;}

        public UpgradeDao(IDB db){
            this._IDB = db;
        }

        public List<UpgradeData> GetUpgradeInfo(){
            
            List<UpgradeData> list = new List<UpgradeData>();

            using (MySqlConnection tMySqlConnection = _IDB.GetMySqlConnection())
            {   
                // tMySqlConnection.Open();

                string query = String.Format("SELECT upgrade_type, upgrade_level, upgrade_amount, upgrade_cost FROM tb_upgrade_info");

                Console.WriteLine(query);

                using(MySqlCommand cmd = (MySqlCommand)tMySqlConnection.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UpgradeData data = new UpgradeData();
                            data.UpgradeType = reader.GetString(0);
                            data.UpgradeLevel = reader.GetInt32(1);
                            data.UpgradeAmount = reader.GetInt32(2);
                            data.UpgradeCost = reader.GetInt32(3);
                            list.Add(data);
                        }
                    }
                }
                
            }
            
            return list;
        }
        
        public UpgradeData GetUpgradeInfo(string UpgradeType, int UpgradeLevel){
            
            UpgradeData tUpgradeData = new UpgradeData();

            using (MySqlConnection tMySqlConnection = _IDB.GetMySqlConnection())
            {   
                // tMySqlConnection.Open();

                // string query = String.Format(@"SELECT upgrade_type, upgrade_level, upgrade_amount, upgrade_cost FROM tb_upgrade_info WHERE upgrade_type = '{0}' AND upgrade_level = {1}", UpgradeType, UpgradeLevel);
                string query = String.Format("SELECT upgrade_type, upgrade_level, upgrade_amount, upgrade_cost FROM tb_upgrade_info WHERE upgrade_type = '{0}' AND upgrade_level = {1}", UpgradeType, UpgradeLevel);

                Console.WriteLine(query);

                using(MySqlCommand tMySqlCommand = (MySqlCommand)tMySqlConnection.CreateCommand())
                {
                    tMySqlCommand.CommandText = query;
                    
                    using (MySqlDataReader tMySqlDataReader = (MySqlDataReader)tMySqlCommand.ExecuteReader())
                    {
                        if (tMySqlDataReader.Read())
                        {
                            tUpgradeData.UpgradeType = tMySqlDataReader.GetString(0);
                            tUpgradeData.UpgradeLevel = tMySqlDataReader.GetInt32(1);
                            // tUpgradeData.UpgradeLevel = tMySqlDataReader.GetInt16(1);
                            tUpgradeData.UpgradeAmount = tMySqlDataReader.GetInt32(2);
                            tUpgradeData.UpgradeCost = tMySqlDataReader.GetInt32(3);

                            return tUpgradeData;
                        }
                    }
                }
            }
            
            return null;
        }

    }
}
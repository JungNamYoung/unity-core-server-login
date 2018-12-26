using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DotnetCoreServer.Models
{
    public interface IRankDao{
        List<RankUser> TotalRank(int Start, int Count);
        List<RankUser> FriendRank(long UserID, List<string> FacebookIDList);
    }

    public class RankDao : IRankDao
    {
        public IDB _IDB {get;}

        public RankDao(IDB db){
            this._IDB = db;
        }

        public List<RankUser> TotalRank(int Start, int Count){
            
            List<RankUser> list = new List<RankUser>();

            using(MySqlConnection tMySqlConnection = _IDB.GetMySqlConnection())
            {   
                string query = String.Format( @"SELECT user_id, facebook_id, facebook_name, facebook_photo_url, point, created_at FROM tb_user ORDER BY point desc LIMIT {0}, {1}", Start, Count );
                // string query = String.Format( "SELECT  user_id, facebook_id, facebook_name, facebook_photo_url, point, created_at  FROM tb_user  ORDER BY point desc  LIMIT {0}, {1}", Start, Count );

                Console.WriteLine(query);

                int rank = 0;

                using(MySqlCommand tMySqlCommand = (MySqlCommand)tMySqlConnection.CreateCommand())
                {
                    tMySqlCommand.CommandText = query;

                    using (MySqlDataReader tMySqlDataReader = (MySqlDataReader)tMySqlCommand.ExecuteReader())
                    {
                        while (tMySqlDataReader.Read())
                        {
                            rank++;
                            RankUser user = new RankUser();
                            user.UserID = tMySqlDataReader.GetInt64(0);
                            user.FacebookID = tMySqlDataReader.GetString(1);
                            user.FacebookName = tMySqlDataReader.GetString(2);
                            user.FacebookPhotoURL = tMySqlDataReader.GetString(3);
                            user.Point = tMySqlDataReader.GetInt32(4);
                            user.CreatedAt = tMySqlDataReader.GetDateTime(5);
                            user.Rank = rank;

                            list.Add(user);
                        }
                    }
                }

                tMySqlConnection.Close();
                
            }
            
            return list;

        }
        
        public List<RankUser> FriendRank(long UserID, List<string> FacebookIDList)
        {
            for(int i = 0; i < FacebookIDList.Count; i++)
            {
                FacebookIDList[i] = string.Format("'{0}'", FacebookIDList[i]);
            }
            
            string StrFacebookIDList = string.Join(",", FacebookIDList);

            List<RankUser> list = new List<RankUser>();

            using(MySqlConnection tMySqlConnection = _IDB.GetMySqlConnection())
            {   
                string query = String.Format( @"SELECT user_id, facebook_id, facebook_name, facebook_photo_url, point, created_at FROM tb_user WHERE facebook_id IN ( {0} ) OR user_id = {1}", StrFacebookIDList, UserID);
                // string query = String.Format("SELECT user_id, facebook_id, facebook_name, facebook_photo_url, point, created_at FROM tb_user WHERE facebook_id IN ( {0} ) OR user_id = {1}", StrFacebookIDList, UserID);

                Console.WriteLine(query);

                int rank = 0;

                using(MySqlCommand tMySqlCommand = (MySqlCommand)tMySqlConnection.CreateCommand())
                {
                    tMySqlCommand.CommandText = query;

                    using (MySqlDataReader tMySqlDataReader = (MySqlDataReader)tMySqlCommand.ExecuteReader())
                    {
                        while (tMySqlDataReader.Read())
                        {
                            rank++;
                            RankUser user = new RankUser();
                            user.UserID = tMySqlDataReader.GetInt64(0);
                            user.FacebookID = tMySqlDataReader.GetString(1);
                            user.FacebookName = tMySqlDataReader.GetString(2);
                            user.FacebookPhotoURL = tMySqlDataReader.GetString(3);
                            user.Point = tMySqlDataReader.GetInt32(4);
                            user.CreatedAt = tMySqlDataReader.GetDateTime(5);
                            user.Rank = rank;

                            list.Add(user);
                        }
                    }
                }
                
                tMySqlConnection.Close();
            }
            return list;
        }



    }
}
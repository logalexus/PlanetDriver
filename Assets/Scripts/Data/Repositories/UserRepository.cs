using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class UserRepository
    {
        private DbConnection _dbConnection; 
        
        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<int> AddUser(string login, string password)
        {
            int id = -1;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Users (Login, Password) values (@Login, @Password)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Login", login);
                    cmd.Parameters.AddWithValue("Password", password);
                    
                    await connect.OpenAsync();

                    if (cmd.ExecuteNonQuery() >= 0)
                    {
                        sql = "SELECT LAST_INSERT_ID() AS ID";
                        cmd.CommandText = sql;
                        var idStr = await cmd.ExecuteScalarAsync();
                        int.TryParse(idStr.ToString(), out id);
                    }
                }
            }
            return id;
        }
        
        public async UniTask<bool> CheckExistEmail(string login)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select exists(" +
                                "select Users.Login " +
                                "from Users " +
                                "where Login = @Login)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Login", login);
                    await connect.OpenAsync();
                    var value = await cmd.ExecuteScalarAsync();
                    result = (long)value == 1;
                }
            }

            return result;
        }
        
        public async UniTask<UserData> GetUserByEmail(string login)
        {
            UserData user = null;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select Users.idUsers, Users.Login, Users.Password " +
                             "from Users " +
                             "where Login = @Login";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("Login", login);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new UserData()
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return user;
        }
        
        public async UniTask<List<LeaderData>> GetLeadersByPlanet(int idPlanetType, int count = 5)
        {
            List<LeaderData> leaders = new List<LeaderData>();
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select Users.Login, Planets.Record " +
                             "from Users, Planets " +
                             "where idPlanetType = @idPlanetType and idUser=idUsers " +
                             "order by Record desc";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("idPlanetType", idPlanetType);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            leaders.Add(new LeaderData()
                            {
                                Login = reader.GetString(0),
                                Record = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }
            return leaders;
        }
    }
}
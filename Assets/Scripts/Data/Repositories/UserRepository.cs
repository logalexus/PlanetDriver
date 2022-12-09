using System;
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

        public async UniTask<int> AddUser(string email, string password)
        {
            int id = -1;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Users (Email, Password) values (@Email, @Password)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Email", email);
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
        
        public async UniTask<bool> CheckExistEmail(string email)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select exists(" +
                                "select Users.Email " +
                                "from Users " +
                                "where Email = @Email)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Email", email);
                    await connect.OpenAsync();
                    var value = await cmd.ExecuteScalarAsync();
                    result = (long)value == 1;
                }
            }

            return result;
        }
        
        public async UniTask<UserData> GetUserByEmail(string email)
        {
            UserData user = null;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select Users.idUsers, Users.Email, Users.Password " +
                             "from Users " +
                             "where Email = @Email";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("Email", email);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new UserData()
                            {
                                Id = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                Password = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return user;
        }
        
    }
}
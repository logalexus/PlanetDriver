using System;
using Cysharp.Threading.Tasks;
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
                    cmd.Parameters.Add("Email", MySqlDbType.String).Value = email;
                    cmd.Parameters.Add("Password", MySqlDbType.String).Value = password;
                    
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
                string sql = "select exists(select Users.Email from Users where Email = @Email)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.Add("Email", MySqlDbType.String).Value = email;
                    
                    await connect.OpenAsync();
                    var value = await cmd.ExecuteScalarAsync();
                    result = (long)value == 1;
                }
            }

            return result;
        }
        
        public async UniTask<bool> Auth(string email)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select exists(select Users.Email from Users where Email = @Email)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.Add("Email", MySqlDbType.String).Value = email;
                    
                    await connect.OpenAsync();
                    var value = await cmd.ExecuteScalarAsync();
                    result = (long)value == 1;
                }
            }

            return result;
        }
    }
}
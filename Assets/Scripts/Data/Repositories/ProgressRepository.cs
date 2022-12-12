using System;
using Cysharp.Threading.Tasks;
using Data.Models;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class ProgressRepository
    {
        private DbConnection _dbConnection; 
        
        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<bool> AddProgress(int userId, int money, int level, int exp)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Progress (Money, Level, Exp, idUser) " +
                             "values (@Money, @Level, @Exp, @idUser)";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Money", money);
                    cmd.Parameters.AddWithValue("Level", level);
                    cmd.Parameters.AddWithValue("Exp", exp);
                    cmd.Parameters.AddWithValue("idUser", userId);
                    
                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }
        
        public async UniTask<ProgressData> GetProgress(int userId)
        {
            ProgressData progress = null;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select Progress.Money, Progress.Level, Progress.Exp " +
                             "from Progress " +
                             "where idUser = @idUser";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("idUser", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            progress = new ProgressData()
                            {
                                Money = reader.GetInt32(0),
                                Level = reader.GetInt32(1),
                                Exp = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }
            return progress;
        }
        
        public async UniTask Update(int userId, ProgressData progressData)
        {
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "update Progress " +
                             "set Money = @Money, Level = @Level, Exp = @Exp " +
                             "where idUser = @idUser";

                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    
                    cmd.Parameters.AddWithValue("Money", progressData.Money);
                    cmd.Parameters.AddWithValue("Level", progressData.Level);
                    cmd.Parameters.AddWithValue("Exp", progressData.Exp);
                    cmd.Parameters.AddWithValue("idUser", userId);

                    await cmd.ExecuteNonQueryAsync();

                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
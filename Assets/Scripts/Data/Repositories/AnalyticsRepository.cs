using System;
using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class AnalyticsRepository
    {
        private DbConnection _dbConnection; 
        
        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<bool> AddAnalytics(int userId, string action, string time)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Analytics (Action, Time, idUser) " +
                             "values (@Action, @Time, @idUser)";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Action", action);
                    cmd.Parameters.AddWithValue("Time", time);
                    cmd.Parameters.AddWithValue("idUser", userId);
                    
                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }
        
        
    }
}
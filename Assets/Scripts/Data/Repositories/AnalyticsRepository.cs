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

        public async UniTask<bool> AddAnalytics(int userId, int idPlanetType)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Planets (Record, idPlanetType, idUser) values (@Record, @idPlanetType, @idUser)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Record", 0);
                    cmd.Parameters.AddWithValue("idPlanetType", idPlanetType);
                    cmd.Parameters.AddWithValue("idUser", userId);
                    
                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }
    }
}
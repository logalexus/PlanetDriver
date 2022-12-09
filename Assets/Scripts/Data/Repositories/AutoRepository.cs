using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class AutoRepository
    {
        private DbConnection _dbConnection; 
        
        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<bool> AddAutoToUser(int userId, int idAutoType)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Autos (idAutoType, idUser) values (@idAutoType, @idUser)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("idAutoType", idAutoType);
                    cmd.Parameters.AddWithValue("idUser", userId);
                    
                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }
        
        public async UniTask<List<AutoData>> GetAvailableAutos(int userId)
        {
            var autos = new List<AutoData>();
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select AutoType.Name, AutoType.Cost " +
                             "from Autos, AutoType " +
                             "where idUser = @idUser and Autos.idAutoType = AutoType.idAutoType)";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("idUser", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autos.Add(new AutoData()
                            {
                                Name = reader.GetString(0),
                                Cost = reader.GetInt32(1),
                            });
                        }
                    }
                }
            }
            return autos;
        }
        
        public async UniTask<List<PlanetData>> GetAllPlanets()
        {
            var planets = new List<PlanetData>();
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select PlanetType.Name, PlanetType.Cost from PlanetType";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planets.Add(new PlanetData()
                            {
                                Name = reader.GetString(0),
                                Cost = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }
            return planets;
        }
        
    }
}
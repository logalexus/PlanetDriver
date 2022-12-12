using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class PlanetRepository
    {
        private DbConnection _dbConnection; 
        
        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<bool> AddPlanetToUser(int userId, int idPlanetType, bool isCurrent)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "insert into Planets (Record, idPlanetType, idUser, IsCurrent) " +
                             "values (@Record, @idPlanetType, @idUser, @IsCurrent)";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("Record", 0);
                    cmd.Parameters.AddWithValue("idPlanetType", idPlanetType);
                    cmd.Parameters.AddWithValue("idUser", userId);
                    cmd.Parameters.AddWithValue("IsCurrent", Convert.ToInt32(isCurrent));

                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }
        
        public async UniTask<List<PlanetData>> GetAvailablePlanets(int userId)
        {
            var planets = new List<PlanetData>();
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select PlanetType.idPlanetType, PlanetType.Name, PlanetType.Cost, " +
                             "PlanetType.TargetLevel, Planets.Record, Planets.IsCurrent " +
                             "from Planets, PlanetType " +
                             "where idUser = @idUser and PlanetType.idPlanetType = Planets.idPlanetType";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("idUser", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planets.Add(new PlanetData()
                            {
                                IdPlanetType = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Cost = reader.GetInt32(2),
                                TargetLevel = reader.GetInt32(3),
                                Record = reader.GetInt32(4),
                                IsCurrent = Convert.ToBoolean(reader.GetInt32(5))
                            });
                        }
                    }
                }
            }
            return planets;
        }
        
        public async UniTask<List<PlanetData>> GetAllPlanetTypes()
        {
            var planets = new List<PlanetData>();
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select PlanetType.idPlanetType, PlanetType.Name, PlanetType.Cost, PlanetType.TargetLevel " +
                             "from PlanetType";
                
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planets.Add(new PlanetData()
                            {
                                IdPlanetType = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Cost = reader.GetInt32(2),
                                TargetLevel = reader.GetInt32(3),
                            });
                        }
                    }
                }
            }
            return planets;
        }
        
        public async UniTask Update(int userId, List<PlanetData> planetsData)
        {
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "update Planets " +
                             "set Record = @Record, IsCurrent = @IsCurrent " +
                             "where idUser = @idUser and idPlanetType = @idPlanetType";

                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    foreach (var planet in planetsData)
                    {
                        cmd.Parameters.AddWithValue("Record", planet.Record);
                        cmd.Parameters.AddWithValue("IsCurrent", Convert.ToBoolean(planet.IsCurrent));
                        cmd.Parameters.AddWithValue("idUser", userId);
                        cmd.Parameters.AddWithValue("idPlanetType", planet.IdPlanetType);

                        await cmd.ExecuteNonQueryAsync();
                    
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
    }
}
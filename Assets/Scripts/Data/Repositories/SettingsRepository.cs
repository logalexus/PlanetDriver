using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Models;
using MySql.Data.MySqlClient;

namespace Data
{
    [Serializable]
    public class SettingsRepository
    {
        private DbConnection _dbConnection;

        public void Init(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async UniTask<bool> AddSettings(int userId, bool soundEnable, bool musicEnable, bool graphicQuality)
        {
            bool result = false;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql =
                    "insert into Settings (SoundEnable, MusicEnable, GraphicQuality, idUser) values (@SoundEnable, @MusicEnable, @GraphicQuality, @idUser)";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("SoundEnable", Convert.ToInt32(soundEnable));
                    cmd.Parameters.AddWithValue("MusicEnable", Convert.ToInt32(musicEnable));
                    cmd.Parameters.AddWithValue("GraphicQuality", Convert.ToInt32(graphicQuality));
                    cmd.Parameters.AddWithValue("idUser", userId);

                    await connect.OpenAsync();

                    result = await cmd.ExecuteNonQueryAsync() > 0;
                }
            }

            return result;
        }

        public async UniTask<SettingsData> GetSettings(int userId)
        {
            SettingsData settings = null;
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "select Settings.SoundEnable, Settings.MusicEnable, Settings.GraphicQuality " +
                             "from Settings " +
                             "where idUser = @idUser";
                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    cmd.Parameters.AddWithValue("idUser", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            settings = new SettingsData()
                            {
                                SoundEnable = Convert.ToBoolean(reader.GetInt32(0)),
                                MusicEnable = Convert.ToBoolean(reader.GetInt32(1)),
                                GraphicQuality = Convert.ToBoolean(reader.GetInt32(2)),
                            };
                        }
                    }
                }
            }

            return settings;
        }

        public async UniTask Update(int userId, SettingsData settingsData)
        {
            using (MySqlConnection connect = new MySqlConnection(_dbConnection.ConnectionString))
            {
                string sql = "update Settings " +
                             "set SoundEnable = @SoundEnable, MusicEnable = @MusicEnable, GraphicQuality = @GraphicQuality " +
                             "where idUser = @idUser";

                using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                {
                    await connect.OpenAsync();
                    
                    cmd.Parameters.AddWithValue("SoundEnable", settingsData.SoundEnable);
                    cmd.Parameters.AddWithValue("MusicEnable", settingsData.MusicEnable);
                    cmd.Parameters.AddWithValue("GraphicQuality", settingsData.GraphicQuality);
                    cmd.Parameters.AddWithValue("idUser", userId);

                    await cmd.ExecuteNonQueryAsync();

                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapsHolder", order = 1)]
public class MapsHolder : ContentHolder<Map>
{
    [Button]
    public async void SyncDatabase()
    {
        dbConnection.Init();
        using (MySqlConnection connect = new MySqlConnection(dbConnection.ConnectionString))
        {
            string sql = "insert into PlanetType (idPlanetType, Name, Cost, TargetLevel) " +
                         "values (@idPlanetType, @Name, @Cost, @TargetLevel) " +
                         "on duplicate key update idPlanetType=@idPlanetType, Name=@Name, " +
                         "Cost=@Cost, TargetLevel=@TargetLevel";

            using (MySqlCommand cmd = new MySqlCommand(sql, connect))
            {
                await connect.OpenAsync();
                foreach (var map in _contents)
                {
                    cmd.Parameters.AddWithValue("idPlanetType", map.Id);
                    cmd.Parameters.AddWithValue("Name", map.Name);
                    cmd.Parameters.AddWithValue("Cost", map.Cost);
                    cmd.Parameters.AddWithValue("TargetLevel", map.TargetLevel);
                    
                    if (await cmd.ExecuteNonQueryAsync() > 0)
                        Debug.Log("Succes");
                    else
                        Debug.Log("Failed");
                    
                    cmd.Parameters.Clear();
                }
            }
        }
    }

    [Button]
    public async void DeleteTypesDatabase()
    {
        dbConnection.Init();
        using (MySqlConnection connect = new MySqlConnection(dbConnection.ConnectionString))
        {
            string sql = "delete from PlanetType";

            using (MySqlCommand cmd = new MySqlCommand(sql, connect))
            {
                await connect.OpenAsync();

                int rows = await cmd.ExecuteNonQueryAsync();
                if (rows > 0)
                    Debug.Log($"{rows} rows deleted - Success");
                else
                    Debug.Log("Failed");
            }
        }
    }
}

[System.Serializable]
public class Map : Content
{
    public Planet Prefab;
    public Material SkyBox;
    public int TargetLevel;
}
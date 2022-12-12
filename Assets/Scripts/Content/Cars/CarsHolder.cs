using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CarsHolder", order = 1)]
public class CarsHolder : ContentHolder<Car>
{
    [Button]
    public async void SyncDatabase()
    {
        dbConnection.Init();
        using (MySqlConnection connect = new MySqlConnection(dbConnection.ConnectionString))
        {
            string sql = "insert into AutoType (idAutoType, Name, Cost) " +
                         "values (@idAutoType, @Name, @Cost) " +
                         "on duplicate key update idAutoType=@idAutoType, Name=@Name, Cost=@Cost";

            using (MySqlCommand cmd = new MySqlCommand(sql, connect))
            {
                await connect.OpenAsync();
                foreach (var map in _contents)
                {
                    cmd.Parameters.AddWithValue("idAutoType", map.Id);
                    cmd.Parameters.AddWithValue("Name", map.Name);
                    cmd.Parameters.AddWithValue("Cost", map.Cost);

                    if (await cmd.ExecuteNonQueryAsync() > 0)
                        Debug.Log($"{map.Name} - Success");
                    else
                        Debug.Log($"{map.Name} - Failed");

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
            string sql = "delete from AutoType";

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
public class Car : Content
{
    public Auto Prefab;
}
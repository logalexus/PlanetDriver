using System;
using Cysharp.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Example.MySql
{
    [Serializable]
    public class DatabaseContext
    {
        [Header("Database Properties")]
        [SerializeField] string Host = "localhost";
        [SerializeField] string User = "root";
        [SerializeField] string Password = "root";
        [SerializeField] string Database = "test";


        public async UniTaskVoid Connect()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Host;
            builder.UserID = User;
            builder.Password = Password;
            builder.Database = Database;
            PopupFactory.Instance.ShowLoadingPopup();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
                {
                    await connection.OpenAsync();
                    Debug.Log("MySQL - Opened Connection");
                    PopupFactory.Instance.ClosePopup();
                }
            }
            catch (MySqlException exception)
            {
                Debug.Log(exception.Message);
                PopupFactory.Instance.ClosePopup();
            }
        }
    }
}
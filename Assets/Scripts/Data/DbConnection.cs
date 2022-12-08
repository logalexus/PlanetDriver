using System;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DbConnection
    {
        [Header("Database Properties")] 
        [SerializeField] string Host = "sql.freedb.tech";
        [SerializeField] string User = "freedb_logalex";
        [SerializeField] string Password = "Ur9Sd2h#5%ERFHB";
        [SerializeField] string Database = "freedb_planetdriver";

        private string _connectionString;

        public string ConnectionString => _connectionString;
    
        public void Init()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Host;
            builder.UserID = User;
            builder.Password = Password;
            builder.Database = Database;
            _connectionString = builder.ToString();
        }
    }
}
using MySqlConnector;
using System.Data;

namespace DaNangTourism.Server.DAL
{
    internal class DAO
    {
        private MySqlConnection _connection;
        private MySqlCommand _command;
        private static DAO? _instance;
        public static DAO Instance
        {
            get
            {
                _instance ??= new DAO("server=127.0.0.1;database=pbl3;uid=root;password=");
                return _instance;
            }
            private set { }
        }
        private DAO(string s)
        {
            _connection = new MySqlConnection(s);
            _command = new MySqlCommand { Connection = _connection };
        }
        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }   
        }
        public MySqlDataReader ExecuteQuery(string query,params MySqlParameter[]? parameters)
        {
            _command.CommandText = query;
            _command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }
            MySqlDataReader reader = _command.ExecuteReader();
            return reader;
        }
        public int ExecuteNonQuery(string query,params MySqlParameter[]? parameters)
        {
            _command.CommandText = query;
            _command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }
            int result = _command.ExecuteNonQuery();
            return result;
        }
        public object? ExecuteScalar(string query,params MySqlParameter[]? parameters)
        {
            _command.CommandText = query;
            _command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                _command.Parameters.AddRange(parameters);
            }
            object? result = _command.ExecuteScalar();
            return result;
        }
    }
}

using MySqlConnector;

namespace DaNangTourism.Server.DAL
{
    internal class DAO
    {
        private MySqlConnection? _con;
        private MySqlCommand? _command;
        private static DAO? _instance;
        public static DAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    string s = "server=127.0.0.1;database=pbl3;uid=root;password=";
                    _instance = new DAO(s);
                }
                return _instance;
            }
            private set { }
        }
        private DAO(string s)
        {
            _con = new MySqlConnection(s);
            _command = new MySqlCommand { Connection = _con };
        }
        public void OpenConnection()
        {
            _con.Open();
        }
        public void CloseConnection()
        {
            _con.Close();
        }
        public MySqlDataReader ExecuteQuery(string query)
        {
            _command.CommandText = query;
            MySqlDataReader reader = _command.ExecuteReader();
            return reader;
        }
        public MySqlDataReader ExecuteQuery(string query, MySqlParameter[]? parameters)
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
        public int ExecuteNonQuery(string query)
        {
            _command.CommandText = query;
            int result = _command.ExecuteNonQuery();
            return result;
        }
        public int ExecuteNonQuery(string query, MySqlParameter[]? parameters)
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
        public object? ExecuteScalar(string query)
        {
            _command.CommandText = query;
            object? result = _command.ExecuteScalar();
            return result;
        }
        public object? ExecuteScalar(string query, MySqlParameter[]? parameters)
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

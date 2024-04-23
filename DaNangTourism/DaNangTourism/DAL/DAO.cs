using MySqlConnector;

namespace DaNangTourism.DAL
{
    internal class DAO
    {
        private MySqlConnection? con;
        private MySqlCommand? Command;
        ~DAO()
        {
            CloseConnection();
        }
        public void OpenConnection()
        {
            if (con == null)
            {
                con = new MySqlConnection("server=127.0.0.1;database=pbl3;uid=root;password=");
                con.Open();
                Command = new MySqlCommand { Connection = con };
            }
        }
        public void CloseConnection()
        {
            if (con != null)
            {
                con.Close();
                Command = null;
                con = null;
            }
        }
        public MySqlDataReader ExecuteQuery(string query, MySqlParameter[]? parameters)
        {
            OpenConnection();
            Command.CommandText = query;
            Command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                Command.Parameters.AddRange(parameters);
            }
            return Command.ExecuteReader();
        }
        public int ExecuteNonQuery(string query, MySqlParameter[]? parameters)
        {
            OpenConnection();
            Command.CommandText = query;
            Command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                Command.Parameters.AddRange(parameters);
            }
            return Command.ExecuteNonQuery();
        }
        public object? ExecuteScalar(string query, MySqlParameter[]? parameters)
        {
            OpenConnection();
            Command.CommandText = query;
            Command.Parameters.Clear();
            if (parameters != null && parameters.Length > 0)
            {
                Command.Parameters.AddRange(parameters);
            }
            return Command.ExecuteScalar();
        }
    }
}

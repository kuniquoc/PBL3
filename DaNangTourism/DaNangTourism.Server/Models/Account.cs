using MySqlConnector;
using System.Net;
using System.Xml.Linq;

namespace DaNangTourism.Server.Models
{
    public enum Permission
    {
        admin = 1,
        user = 0
    }
    public class Account
    {
        private int _id;
        private string? _name;
        private DateTime _birthday;
        private string? _email;
        private string? _username;
        private string? _password;
        private Permission _permission;
        private string? _avatar;

        public int Id { get { return _id; } set { _id = value; } }
        public string? Name { get { return _name; } set { _name = value; } }
        public DateTime Birthday { get { return _birthday; } set { _birthday = value; } }
        public string? Email { get { return _email; } set { _email = value; } }
        public string? Username { get { return _username; } set { _username = value; } }
        public string? Password { get { return _password; } set { _password = value; } }
        public Permission Permission { get { return _permission; } set { _permission = value; } }
        public string? Avatar { get { return _avatar; } set { _avatar = value; } }

        public Account() { }

        public Account(int id, string name, DateTime birthday, string email, string username, string password, Permission permission, string? avatar)
        {
            _id = id;
            _name = name;
            _birthday = birthday;
            _email = email;
            _username = username;
            _password = password;
            _permission = permission;
            _avatar = avatar;
        }

        public Account(MySqlDataReader reader)
        {
            _id = reader.GetInt32("user_id");
            _name = reader.GetString("full_name");
            _birthday = reader.GetDateTime("birthday");
            _email = reader.GetString("email");
            _username = reader.GetString("user_name");
            _password = reader.GetString("password");
            _permission = Enum.Parse<Permission>(reader.GetString("permission"));
            _avatar = reader.GetString("avatar_url");
        }
    }
}

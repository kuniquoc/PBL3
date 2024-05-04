using MySqlConnector;
using System.Net;
using System.Xml.Linq;

namespace DaNangTourism.Server.Models
{
    public class Account
    {
        private int _id;
        private string _username;
        private string _password;
        private string _email;
        private bool _status;
        private string _name;
        private string _address;
        private DateTime _dateOfBirth;
        //gender: 0 - female, 1 - male
        private bool _gender;

        public int Id { get { return _id; } set { _id = value; } }
        public string Username { get { return _username; } set { _username = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public bool Status { get { return _status; } set { _status = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Address { get { return _address; } set { _address = value; } }
        public DateTime DateOfBirth { get { return _dateOfBirth; } set { _dateOfBirth = value; } }
        public bool Gender { get { return _gender; } set { _gender = value; } }

        public Account()
        {
            _id = 0;
            _username = "";
            _password = "";
            _email = "";
            _status = false;
            _name = "";
            _address = "";
            _dateOfBirth = DateTime.Now;
            _gender = true;
        }

        public Account(int id, string username, string password, string email, bool status, string name, string address, DateTime dateOfBirth, bool  gender)
        {
            _id = id;
            _username = username;
            _password = password;
            _email = email;
            _status = status;
            _name = name;
            _address = address;
            _dateOfBirth = dateOfBirth;
            _gender = gender;
        }

        public Account(MySqlDataReader reader)
        {
            _id = reader.GetInt32("account_id");
            _username = reader.GetString("username");
            _password = reader.GetString("password");
            _email = reader.GetString("email");
            _status = reader.GetBoolean("status");
            _name = reader.GetString("name");
            _address = reader.GetString("address");
            _dateOfBirth = reader.GetDateTime("date_of_birth");
            _gender = reader.GetBoolean("gender");
        }
    }
}

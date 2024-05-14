using MySqlConnector;
using System.Data;
using System.Net;
using System.Text;
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
        private byte [] _passwordHash;
        private byte [] _passwordSalt;
        private Permission _permission;
        private string? _avatar;

        //passwordHash và passwordSalt sử dụng HMACSHA512

        public int Id { get { return _id; } set { _id = value; } }
        public string? Name { get { return _name; } set { _name = value; } }
        public DateTime Birthday { get { return _birthday; } set { _birthday = value; } }
        public string? Email { get { return _email; } set { _email = value; } }
        public string? Username { get { return _username; } set { _username = value; } }
        public byte[] PasswordHash { get { return _passwordHash; } set { _passwordHash = value; } }
        public byte[] PasswordSalt { get { return _passwordSalt; } set { _passwordSalt = value; } }
        public Permission Permission { get { return _permission; } set { _permission = value; } }
        public string? Avatar { get { return _avatar; } set { _avatar = value; } }

        public Account() { }

        public Account(int id, string name, DateTime birthday, string email, string username, byte[] passwordHash,
            byte[] passwordSalt, Permission permission, string? avatar)
        {
            _id = id;
            _name = name;
            _birthday = birthday;
            _email = email;
            _username = username;
            _passwordHash = passwordHash;
            _passwordSalt = passwordSalt;
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
            _passwordHash = (byte[])(reader.GetValue("password_hash"));
            _passwordSalt = (byte[])(reader.GetValue("password_salt"));
            _permission = Enum.Parse<Permission>(reader.GetString("permission"));
            _avatar = reader.GetString("avatar_url");
        }
    }

    //Tạo model để lưu thông tin tài khoản khi đăng nhập
    public class AccountLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    //Tạo model để lưu thông tin tài khoản khi đăng ký
    public class AccountRegister
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string avartarDefault = "https://th.bing.com/th/id/OIP.3U017h9GAnFM3aRkV-WLiwHaHa?w=800&h=800&rs=1&pid=ImgDetMain";
    }

    //Tạo model để lưu thông tin tài khoản khi thay đổi thông tin cá nhân
    public class AccountUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
    }
}

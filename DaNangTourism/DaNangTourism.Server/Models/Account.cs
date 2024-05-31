using MySqlConnector;
using System.Data;

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
        private string _name;
        private DateTime _birthday;
        private string _email;
        private byte[] _passwordHash;
        private byte[] _passwordSalt;
        private Permission _permission;
        private string? _avatar;
        private DateTime _createdAt;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public DateTime Birthday { get { return _birthday; } set { _birthday = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public byte[] PasswordHash { get { return _passwordHash; } set { _passwordHash = value; } }
        public byte[] PasswordSalt { get { return _passwordSalt; } set { _passwordSalt = value; } }
        public Permission Permission { get { return _permission; } set { _permission = value; } }
        public string? Avatar { get { return _avatar; } set { _avatar = value; } }
        public DateTime CreatedAt { get { return _createdAt; } set { _createdAt = value; } }

        public Account()
        {
            _id = 0;
            // Tên ngẫu nhiên
            _name = "User" + new Random().Next(1000, 9999);
            _birthday = new DateTime(2000, 1, 1);
            _email = "";
            _passwordHash = [];
            _passwordSalt = [];
            _permission = Permission.user;
            _avatar = "https://cdn3.iconfinder.com/data/icons/web-design-and-development-2-6/512/87-1024.png";
            _createdAt = DateTime.Now;
        }

        public Account(int id, string name, DateTime birthday, string email, byte[] passwordHash,
            byte[] passwordSalt, Permission permission, string? avatar, DateTime createdAt)
        {
            _id = id;
            _name = name;
            _birthday = birthday;
            _email = email;
            _passwordHash = passwordHash;
            _passwordSalt = passwordSalt;
            _permission = permission;
            _avatar = avatar;
            _createdAt = createdAt;
        }

        public Account(MySqlDataReader reader)
        {
            _id = reader.GetInt32("user_id");
            _name = reader.GetString("full_name");
            _birthday = reader.GetDateTime("birthday");
            _email = reader.GetString("email");
            _passwordHash = (byte[])(reader.GetValue("password_hash"));
            _passwordSalt = (byte[])(reader.GetValue("password_salt"));
            _permission = Enum.Parse<Permission>(reader.GetString("permission"));
            _avatar = reader.GetString("avatar_url");
            _createdAt = reader.GetDateTime("created_at");
        }
    }

    public class AccountLoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }

    public class AccountRegisterModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AccountUpdateModel
    {
        public required string Name { get; set; }
        public required string Avatar { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class AccountChangePasswordModel
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? avatar { get; set; }
        public Author() { }
        public Author(Account account)
        {
            id = account.Id;
            name = account.Name;
            avatar = account.Avatar;
        }
        public Author(MySqlDataReader reader)
        {
            id = reader.GetInt32("id");
            name = reader.GetString("name");
            avatar = reader.GetString("avatar");
        }
    }
}

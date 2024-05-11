using MySqlConnector;
using DaNangTourism.Server.Models;
using System.Security.Cryptography;
using System.Security;
using System.Xml.Linq;

namespace DaNangTourism.Server.DAL
{
    public class AccountDAO
    {
        private readonly DAO _dao;
        private static AccountDAO _instance;
        public static AccountDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccountDAO();
                }
                return _instance;
            }
            private set { }
        }

        private AccountDAO()
        {
            _dao = DAO.Instance;
        }

        //Kiểm tra tài khoản tồn tại
        public bool CheckAccountExist(string username)
        {
            string sql = "Select * from users where user_name = @username";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@username", username) };
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            bool result = reader.Read();
            reader.Close();
            _dao.CloseConnection(); 
            return result;
        }

        //Thêm tài khoản
        public int AddAccount(Account account)
        {
            string sql = "Insert into users(full_name, birthday, email, user_name, password, permission, avatar_url) values(@name, @birthday, @email, @username, @password, @permission, @avatar)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@name", account.Name),
                new("@birthday", account.Birthday),
                new("@email", account.Email),
                new("@username", account.Username),
                new("@password", account.Password),
                new("@permission", account.Permission),
                new("@avatar", account.Avatar)
            };
            return _dao.ExecuteNonQuery(sql, parameters);
        }

        //Cập nhật thông tin tài khoản
        public int UpdateAccountÌnformation(Account account)
        {
            string sql = "Update users set full_name = @name, birthday = @birthday, avatar_url = @avatar where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@name", account.Name),
                new("@birthday", account.Birthday),
                new("@avatar", account.Avatar),
                new("@id", account.Id)
            };
            return _dao.ExecuteNonQuery(sql, parameters);
        }
        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            string sql = "Delete from users where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@id", id) };
            return _dao.ExecuteNonQuery(sql, parameters);
        }

        //Lấy toàn bộ tài khoản
        public Dictionary<int, Account> GetAllAccounts()
        {
            string sql = "Select * from users";
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql);
            Dictionary<int, Account> accounts = new();
            while (reader.Read())
            {
                Account account = new(reader);
                accounts.Add(account.Id, account);
            }
            _dao.CloseConnection();
            return accounts;
        }
        //Lấy tài khoản theo id
        public Account GetAccountById(int id)
        {
            string sql = "Select * from users where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@id", id) };
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            Account account = new();
            if (reader.Read())
            {
                account = new Account(reader);
            }
            _dao.CloseConnection();
            return account;
        }

        //Lấy tài khoản theo username
        public Account GetAccountByUsername(string username)
        {
            string sql = "Select * from users where user_name = @username";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@username", username) };
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            Account account = new();
            if (reader.Read())
            {
                account = new Account(reader);
            }
            _dao.CloseConnection();
            return account;
        }
        /*
        
        //Đổi mật khẩu
        public int ChangePassword(int id, string newPassword)
        {
            DAO dao = new();
            string sql = "Update users set password = @password where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@password", newPassword),
                new("@id", id)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Đổi email
        public int ChangeEmail(int id, string newEmail)
        {
            DAO dao = new();
            string sql = "Update users set email = @email where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@email", newEmail),
                new("@id", id)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            DAO dao = new();
            string sql = "Delete from users where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Kiểm tra tài khoản tồn tại
        public bool CheckAccountExist(string username)
        {
            DAO dao = new();
            string sql = "Select * from users where user_name = @username";
            MySqlParameter[] parameters = new MySqlParameter[] { new("@username", username) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            return reader.Read();
        }

        //Kiểm tra đăng nhập
        public Account? CheckLogin(string username, string password)
        {
            DAO dao = new();
            string sql = "Select * from users where user_name = @username and password = @password" ;
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@username", username),
                new("@password", password)
            };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                Account account = new(reader);
                return account;
            }
            return null;
        }

        //Kiểm tra đăng ký
        public int CheckRegister(string username, string email)
        {
            DAO dao = new();
            string sql = "Select * from users where user_name = @username or email = @email";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new("@username", username),
                new("@email", email)
            };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                if (reader.GetString("user_name") == username) return 1; //Trùng username
                else return 2; //Trùng email
            }
            return 0; //Không trùng
        }
        */

    }
}

using MySqlConnector;
using DaNangTourism.Server.Models;
using System.Security.Cryptography;
using System.Security;
using System.Xml.Linq;

namespace DaNangTourism.Server.DAL
{
    public class AccountDAO
    {
        //Lấy toàn bộ tài khoản
        public Dictionary<int, Account> GetAllAccounts()
        {
            DAO dao = new DAO();
            Dictionary<int, Account> accounts = new Dictionary<int, Account>();
            string sql = "Select * from users";
            MySqlDataReader reader = dao.ExecuteQuery(sql, null);
            while (reader.Read())
            {
                Account account = new Account(reader);
                accounts.Add(account.Id, account);
            }
            return accounts;
        }

        //Lấy tài khoản theo id
        public Account GetAccountById(int id)
        {
            DAO dao = new DAO();
            string sql = "Select * from users where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            Account account = new Account();
            if (reader.Read())
            {
                account = new Account(reader);
            }
            return account;
        }

        //Thêm tài khoản
        public int AddAccount(Account account)
        {
            DAO dao = new DAO();
            string sql = "Insert into users(full_name, birthday, email, user_name, password, permission, avatar_url) values(@name, @birthday, @email, @username, @password, @permission, @avatar)";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", account.Name),
                new MySqlParameter("@birthday", account.Birthday),
                new MySqlParameter("@email", account.Email),
                new MySqlParameter("@username", account.Username),
                new MySqlParameter("@password", account.Password),
                new MySqlParameter("@permission", account.Permission),
                new MySqlParameter("@avatar", account.Avatar)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Cập nhật thông tin tài khoản
        public int UpdateAccountÌnformation(Account account)
        {
            DAO dao = new DAO();
            string sql = "Update users set full_name = @name, birthday = @birthday, avatar_url = @avatar where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", account.Name),
                new MySqlParameter("@birthday", account.Birthday),
                new MySqlParameter("@avatar", account.Avatar),
                new MySqlParameter("@id", account.Id)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Đổi mật khẩu
        public int ChangePassword(int id, string newPassword)
        {
            DAO dao = new DAO();
            string sql = "Update users set password = @password where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@password", newPassword),
                new MySqlParameter("@id", id)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Đổi email
        public int ChangeEmail(int id, string newEmail)
        {
            DAO dao = new DAO();
            string sql = "Update users set email = @email where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@email", newEmail),
                new MySqlParameter("@id", id)
            };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            DAO dao = new DAO();
            string sql = "Delete from users where user_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Kiểm tra tài khoản tồn tại
        public bool CheckAccountExist(string username)
        {
            DAO dao = new DAO();
            string sql = "Select * from users where user_name = @username";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@username", username) };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            return reader.Read();
        }

        //Kiểm tra đăng nhập
        public Account? CheckLogin(string username, string password)
        {
            DAO dao = new DAO();
            string sql = "Select * from users where user_name = @username and password = @password" ;
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password)
            };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                Account account = new Account(reader);
                return account;
            }
            return null;
        }

        //Kiểm tra đăng ký
        public int CheckRegister(string username, string email)
        {
            DAO dao = new DAO();
            string sql = "Select * from users where user_name = @username or email = @email";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@email", email)
            };
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            if (reader.Read())
            {
                if (reader.GetString("user_name") == username) return 1; //Trùng username
                else return 2; //Trùng email
            }
            return 0; //Không trùng
        }

    }
}

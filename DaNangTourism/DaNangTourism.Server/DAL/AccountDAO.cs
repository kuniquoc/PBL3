using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class AccountDAO
    {
        //Lấy toàn bộ tài khoản
        public Dictionary<int, Account> GetAllAccounts()
        {
            DAO dao = new DAO();
            Dictionary<int, Account> accounts = new Dictionary<int, Account>();
            string sql = "Select * from accounts";
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
            string sql = "Select * from accounts where account_id = @id";
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
            string sql = "Insert into accounts(account_id, username, password, email, status, name, address, date_of_birth)"
                + " values(@account_id, @username, @password, @email, @status, @name, @address, @date_of_birth)";
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("@account_id", account.Id);
            parameters[1] = new MySqlParameter("@username", account.Username);
            parameters[2] = new MySqlParameter("@password", account.Password);
            parameters[3] = new MySqlParameter("@email", account.Email);
            parameters[4] = new MySqlParameter("@status", account.Status);
            parameters[5] = new MySqlParameter("@name", account.Name);
            parameters[6] = new MySqlParameter("@address", account.Address);
            parameters[7] = new MySqlParameter("@date_of_birth", account.DateOfBirth);
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Cập nhật tài khoản
        public int UpdateAccount(Account account)
        {
            DAO dao = new DAO();
            string sql = "Update accounts set username = @username, password = @password, email = @email, status = @status, name = @name, address = @address, date_of_birth = @date_of_birth where account_id = @account_id";
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("@username", account.Username);
            parameters[1] = new MySqlParameter("@password", account.Password);
            parameters[2] = new MySqlParameter("@email", account.Email);
            parameters[3] = new MySqlParameter("@status", account.Status);
            parameters[4] = new MySqlParameter("@name", account.Name);
            parameters[5] = new MySqlParameter("@address", account.Address);
            parameters[6] = new MySqlParameter("@date_of_birth", account.DateOfBirth);
            parameters[7] = new MySqlParameter("@account_id", account.Id);
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            DAO dao = new DAO();
            string sql = "Delete from accounts where account_id = @id";
            MySqlParameter[] parameters = new MySqlParameter[] { new MySqlParameter("@id", id) };
            return dao.ExecuteNonQuery(sql, parameters);
        }

        //Kiểm tra tài khoản tồn tại
        public bool CheckAccountExist(string username, string password)
        {
            DAO dao = new DAO();
            string sql = "Select * from accounts where username = @username and password = @password";
            MySqlParameter[] parameters = new MySqlParameter[2];
            parameters[0] = new MySqlParameter("@username", username);
            parameters[1] = new MySqlParameter("@password", password);
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            return reader.Read();
        }

        //Kiểm tra tài khoản tồn tại
        public bool CheckAccountExist(string username)
        {
            DAO dao = new DAO();
            string sql = "Select * from accounts where username = @username";
            MySqlParameter[] parameters = new MySqlParameter[1];
            parameters[0] = new MySqlParameter("@username", username);
            MySqlDataReader reader = dao.ExecuteQuery(sql, parameters);
            return reader.Read();
        }
    }
}

using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public class AccountDAO
    {
        private readonly DAO _dao;
        private static AccountDAO? _instance;
        public static AccountDAO Instance
        {
            get
            {
                _instance ??= new AccountDAO();
                return _instance;
            }
            private set { }
        }

        private AccountDAO()
        {
            _dao = DAO.Instance;
        }

        //Thêm tài khoản
        public int AddAccount(Account account)
        {
            string sql = "Insert into users(full_name, birthday, email, password_hash, password_salt , permission, avatar_url, created_at) " +
                "values(@name, @birthday, @email, @passwordHash, @passwordSalt, @permission, @avatar, @createdat)";
            MySqlParameter[] parameters =
            [
                new("@name", account.Name),
                new("@birthday", new DateTime(2000,1,1)),
                new("@email", account.Email),
                new("@passwordHash", account.PasswordHash),
                new("@passwordSalt", account.PasswordSalt),
                new("@permission", Permission.user.ToString()),
                new("@avatar", account.Avatar),
                new("@createdat", account.CreatedAt)
            ];
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }

        //Cập nhật thông tin tài khoản
        public int UpdateAccount(Account account)
        {
            string sql = "Update users set full_name = @name, birthday = @birthday, email = @email, password_hash = @passwordhash, password_salt = @passwordsalt, permission = @permission, avatar_url = @avatar where user_id = @id";
            MySqlParameter[] parameters =
            [
                new("@name", account.Name),
                new("@birthday", account.Birthday),
                new("@email", account.Email),
                new("@passwordhash", account.PasswordHash),
                new("@passwordsalt", account.PasswordSalt),
                new("@permission", account.Permission.ToString()),
                new("@avatar", account.Avatar),
                new("@id", account.Id)
            ];
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            _dao.CloseConnection();
            return result;
        }
        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            string sql = "Delete from users where user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            _dao.OpenConnection();
            int result = _dao.ExecuteNonQuery(sql, parameters);
            return result;
        }

        //Lấy toàn bộ tài khoản
        public Dictionary<int, Account> GetAllAccounts()
        {
            string sql = "Select * from users";
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql);
            Dictionary<int, Account> accounts = [];
            while (reader.Read())
            {
                Account account = new(reader);
                accounts.Add(account.Id, account);
            }
            _dao.CloseConnection();
            return accounts;
        }

    }
}

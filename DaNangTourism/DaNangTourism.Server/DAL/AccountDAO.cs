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
                "values(@search, @birthday, @email, @passwordHash, @passwordSalt, @permission, @avatar, @createdat)";
            MySqlParameter[] parameters =
            [
                new("@search", account.Name),
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
            string sql = "Update users set full_name = @search, birthday = @birthday, email = @email, password_hash = @passwordhash, password_salt = @passwordsalt, permission = @permission, avatar_url = @avatar where user_id = @id";
            MySqlParameter[] parameters =
            [
                new("@search", account.Name),
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

        //Lấy tài khoản theo id
        public Account? GetAccountById(int id)
        {
            string sql = "Select * from users where user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            Account? account = null;
            if (reader.Read())
            {
                account = new(reader);
            }
            _dao.CloseConnection();
            return account;
        }

        //Lấy tài khoản theo email
        public Account? GetAccountByEmail(string email)
        {
            string sql = "Select * from users where email = @email";
            MySqlParameter[] parameters = [new("@email", email)];
            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);
            Account? account = null;
            if (reader.Read())
            {
                account = new(reader);
            }
            _dao.CloseConnection();
            return account;
        }

        /*
        page: Number of page (Default: 1)
        limit: Number of items per page (Default: 15)
        search: Search by name
        role: Filter by role (user, admin, all) (Default: user)
        sortBy: Sort by (name, created_at) (Default: created_at)
        sortType: Sort type (asc, desc) (Default: desc)
        */
        public Dictionary<int, Account> SearchAccount(string? search, int page = 1, int limit = 15, string role = "user", string sortBy = "created_at", string sortType = "desc")
        {
            // Validate sortBy and sortType parameters to prevent SQL injection
            var validSortColumns = new List<string> { "full_name", "created_at" };
            var validSortOrders = new List<string> { "asc", "desc" };

            if (!validSortColumns.Contains(sortBy.ToLower()))
            {
                sortBy = "created_at"; // default sort column
            }

            if (!validSortOrders.Contains(sortType.ToLower()))
            {
                sortType = "desc"; // default sort order
            }

            // Construct the base SQL query
            string sql = "SELECT * FROM users WHERE full_name LIKE @name";

            // Apply role filter if specified
            if (role.ToLower() != "all")
            {
                sql += " AND permission = @role";
            }

            // Append ORDER BY and LIMIT clauses
            sql += $" ORDER BY {sortBy} {sortType} LIMIT @offset, @limit";

            // Define the parameters
            MySqlParameter[] parameters = {
                new MySqlParameter("@name", "%" + search + "%"),
                new MySqlParameter("@offset", (page - 1) * limit),
                new MySqlParameter("@limit", limit)
            };

            // Include role parameter only if role filter is applied
            if (role.ToLower() != "all")
            {
                parameters = parameters.Append(new MySqlParameter("@role", role)).ToArray();
            }

            _dao.OpenConnection();
            MySqlDataReader reader = _dao.ExecuteQuery(sql, parameters);

            var accounts = new Dictionary<int, Account>();
            while (reader.Read())
            {
                var account = new Account(reader);
                accounts.Add(account.Id, account);
            }

            _dao.CloseConnection();
            return accounts;
        }
    }
}

using MySqlConnector;
using DaNangTourism.Server.Models;

namespace DaNangTourism.Server.DAL
{
    public interface IAccountRepository
    {
        int AddAccount(Account account);
        int UpdateAccount(Account account);
        int DeleteAccount(int id);
        Dictionary<int, Account> GetAllAccounts();
        Account? GetAccountById(int id);
        Account? GetAccountByEmail(string email);
        Dictionary<int, Account> SearchAccount(string? search, int page = 1, int limit = 15, string role = "user", string sortBy = "created_at", string sortType = "desc");
        int GetTotalAccounts(string? search, string role = "user");
        Permission GetRoleById(int id);
        string GetUserNameById(int id);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionstring;
        public AccountRepository(string connectionstring)
        {
            _connectionstring = connectionstring;
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
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
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
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }
        //Xóa tài khoản
        public int DeleteAccount(int id)
        {
            string sql = "Delete from users where user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }

        //Lấy toàn bộ tài khoản
        public Dictionary<int, Account> GetAllAccounts()
        {
            string sql = "Select * from users";
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var accounts = new Dictionary<int, Account>();
                        while (reader.Read())
                        {
                            var account = new Account(reader);
                            accounts.Add(account.Id, account);
                        }
                        return accounts;
                    }
                }
            }
        }

        //Lấy tài khoản theo id
        public Account? GetAccountById(int id)
        {
            string sql = "Select * from users where user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Account(reader);
                        }
                        else return null;
                    }
                }
            }
        }

        //Lấy tài khoản theo email
        public Account? GetAccountByEmail(string email)
        {
            string sql = "Select * from users where email = @email";
            MySqlParameter[] parameters = [new("@email", email)];
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Account(reader);
                        }
                        else return null;
                    }
                }
            }
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

            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        var accounts = new Dictionary<int, Account>();
                        while (reader.Read())
                        {
                            var account = new Account(reader);
                            accounts.Add(account.Id, account);
                        }
                        return accounts;
                    }
                }
            }
        }

        //Lấy tổng số tài khoản khi tìm kiếm
        public int GetTotalAccounts(string? search, string role = "user")
        {
            string sql = "SELECT COUNT(*) FROM users WHERE full_name LIKE @name";

            // Apply role filter if specified
            if (role.ToLower() != "all")
            {
                sql += " AND permission = @role";
            }

            // Define the parameters
            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", "%" + search + "%")
            };

            // Include role parameter only if role filter is applied
            if (role.ToLower() != "all")
            {
                parameters = parameters.Append(new MySqlParameter("@role", role)).ToArray();
            }

            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public Permission GetRoleById(int id)
        {
            string sql = "SELECT permission FROM users WHERE user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        return Enum.Parse<Permission>(reader.GetString("permission"));
                    }
                }
            }
        }

        public string GetUserNameById(int id)
        {
            string sql = "SELECT full_name FROM users WHERE user_id = @id";
            MySqlParameter[] parameters = [new("@id", id)];
            using (var connection = new MySqlConnection(_connectionstring))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.GetString("full_name");
                    }
                }
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string LastLogin { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }

        public User()
        {
            Id = "";
            Username = "";
            FullName = "";
            Email = "";
            Password = "";
            Phone = "";
            LastLogin = "";
            Token = "";
            IsAdmin = false;
        }

        public User(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            Username = reader[reader.GetOrdinal("USERNAME")].ToString() ?? "";
            FullName = reader[reader.GetOrdinal("FULL_NAME")].ToString() ?? "";
            Email = reader[reader.GetOrdinal("EMAIL")].ToString() ?? "";
            Password = reader[reader.GetOrdinal("PASSWORD")].ToString() ?? "";
            Phone = reader[reader.GetOrdinal("PHONE")].ToString() ?? "";
            LastLogin = reader[reader.GetOrdinal("LAST_LOGIN")].ToString() ?? "";
            Token = reader[reader.GetOrdinal("TOKEN")].ToString() ?? "";
            IsAdmin = DatabaseConnection.ConvertToBoolean(reader[reader.GetOrdinal("IS_ADMIN")].ToString() ?? "false");
        }

        internal int Update()
        {
            string query = "UPDATE TBL_USERS SET LAST_LOGIN = @0, TOKEN = @1 WHERE USERNAME = @2";
            return DatabaseConnection.NonQuery(query, LastLogin, Token, Username);
        }

        public User(string username, string fullName, string email, string password, string phone, string id = "", string lastLogin = "", string token = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Username = username;
            FullName = fullName;
            Email = email;
            Password = password;
            Phone = phone;
            LastLogin = lastLogin;
            Token = token;
        }

        public static List<User> GetUsersList()
        {
            try
            {
                var users = new List<User>();
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery);
                while (reader?.Read() ?? false)
                {
                    User user = new User(reader);
                    users.Add(user);
                }
                return users;
            }
            catch (Exception) { throw; }
        }

        internal static User? GetByUsername(string username)
        {
            string whereString = $" WHERE USERNAME = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, username);
            if (reader?.Read() ?? false)
            {
                User user = new User(reader);
                return user;
            }
            reader?.Close();
            return null;
        }

        internal static User? GetByUserId(string userId)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, userId);
            if (reader?.Read() ?? false)
            {
                User user = new User(reader);
                return user;
            }
            reader?.Close();
            return null;
        }

        static string BaseQuery = "SELECT ID, USERNAME, FULL_NAME, EMAIL, PASSWORD, PHONE, LAST_LOGIN, TOKEN, IS_ADMIN FROM TBL_USERS ";

        public static string Login(LoginCriteria login)
        {

            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return "";
            }

            User? currentUser = GetByUsername(login.Username);
            if (currentUser != null && SecurePassword.Verify(login.Password, currentUser.Password))
            {
                string token = Authentication.Encrypt($"{currentUser.Id}##{login.Username}##{Helper.DateTimeToChar14(DateTime.Now)}");
                currentUser.LastLogin = Helper.DateTimeToChar14(DateTime.Now);
                currentUser.Token = token;
                if (currentUser.Update() == 1)
                {
                    return token;
                }
            }

            return "";
        }

        internal bool IsLogin()
        {
            var tokenDate = Helper.Char14ToDateTime(LastLogin, DateTime.MinValue);

            if (tokenDate.AddHours(3) > DateTime.Now)
            {
                return true;
            }
            else
            {
                Logout(Username);
                return false;
            }
        }

        internal static bool IsExists(string username)
        {
            return GetByUsername(username) != null;
        }

        //public static bool ChangePassword(string oldPassword, string newPassword, string userId, out string errorMessage)
        //{
        //    string whereString = $" WHERE FLDUID = @0";
        //    SqlDataReader reader = DatabaseConnection.Query(BaseQuery + whereString, userId);
        //    if (reader?.Read() ?? false)
        //    {
        //        string passwordHash = reader[reader.GetOrdinal("FLDPASSWORDHASH")].ToString();
        //        if (SecurePassword.Verify(oldPassword, passwordHash))
        //        {
        //            string query = "UPDATE TBLUSERS SET FLDPASSWORDHASH = @0 WHERE FLDUID = @1";
        //            string newPasswordHash = SecurePassword.Hash(newPassword);
        //            errorMessage = "";
        //            return DatabaseConnection.NonQuery(query, newPasswordHash, userId) == 1;
        //        }
        //        else
        //        {
        //            errorMessage = "كلمة المرور الحالية غير صحيحية";
        //            return false;
        //        }
        //    }
        //    errorMessage = "حدث خطأ ما";
        //    Logger.Write(errorMessage);
        //    return false;
        //}

        internal static bool Logout(string username)
        {
            try
            {
                User? currentUser = GetByUsername(username);
                if (currentUser != null)
                {
                    currentUser.LastLogin = "";
                    currentUser.Token = "";
                    if (currentUser.Update() == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal int Insert()
        {
            string query = "Insert into TBL_USERS (ID, USERNAME, FULL_NAME, EMAIL, PASSWORD, PHONE, LAST_LOGIN, TOKEN, IS_ADMIN) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
            int result = DatabaseConnection.NonQuery(query, Id, Username, FullName, Email, SecurePassword.Hash(Password), Phone, LastLogin, Token, IsAdmin.ToString());
            return result;
        }
    }
}

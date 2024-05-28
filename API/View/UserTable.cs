namespace OnlineStore
{
    public class UserTable
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }

        public UserTable(User user)
        {
            Id = user.Id;
            Username = user.Username;
            FullName = user.FullName;
            Phone = user.Phone;
            IsAdmin = user.IsAdmin;
        }

        public UserTable()
        {
            Username = string.Empty;
            FullName = string.Empty;
            Phone = string.Empty;
            IsAdmin = false;
            Id = "";
        }

        public UserTable(string id, string username, string fullName, string phone, string birthday, string bill, bool isAdmin)
        {
            Id = id;
            Username = username;
            FullName = fullName;
            Phone = phone;
            IsAdmin = isAdmin;
        }
    }
}

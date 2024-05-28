namespace OnlineStore
{
    [Serializable]
    public class Register
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }


        public Register(string email, string username, string password, string fullName, string phoneNumber)
        {
            this.email = email;
            this.username = username;
            this.password = password;
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }

        public Register()
        {
            email = "";
            username = "";
            password = "";
            fullName = "";
            phoneNumber = "";
        }
    }
}

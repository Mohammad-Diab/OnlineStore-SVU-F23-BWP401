namespace OnlineStore
{
    public class DeleveryRequest
    {
        public string PayMethod { get; set; }
        public string CardId { get; set; }
        public string ReciverName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Instructions { get; set; }

        public DeleveryRequest(string payMethod, string cardId, string reciverName, string phone, string email, string address, string instructions)
        {
            PayMethod = payMethod;
            CardId = cardId;
            ReciverName = reciverName;
            Phone = phone;
            Email = email;
            Address = address;
            Instructions = instructions;
        }

        public DeleveryRequest()
        {
            PayMethod = string.Empty;
            CardId = string.Empty;
            ReciverName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            Instructions = string.Empty;
        }
    }
}

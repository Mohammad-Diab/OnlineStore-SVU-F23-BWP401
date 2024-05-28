namespace OnlineStore
{
    public class CardRequest
    {
        public string Number { get; set; }
        public string HolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvc { get; set; }

        public CardRequest(string number, string holderName, string expiryDate, string cvc)
        {
            Number = number;
            HolderName = holderName;
            ExpiryDate = expiryDate;
            Cvc = cvc;
        }

        public CardRequest()
        {
            Number = string.Empty;
            HolderName = string.Empty;
            ExpiryDate = string.Empty;
            Cvc = string.Empty;
        }
    }
}

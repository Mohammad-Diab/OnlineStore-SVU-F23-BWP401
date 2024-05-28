namespace OnlineStore
{
    public class CardTable
    {
        public string Id { get; set; }
        public string LastFourDigits { get; set; }
        public string HolderName { get; set; }
        public string ExpiryDate { get; set; }
        public bool IsAdmin { get; set; }

        public CardTable(Card card)
        {
            Id = card.Id;
            LastFourDigits = card.LastFourDigits;
            HolderName = card.HolderName;
            ExpiryDate = card.ExpiryDate;
            IsAdmin = false;
        }
    }
}

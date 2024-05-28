using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore
{
    public class Card
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string HolderName { get; set; }
        public string LastFourDigits { get; set; }
        public string ExpiryDate { get; set; }
        public string CardNumber { get; set; }

        static string BaseQuery = "SELECT ID, USER_ID, HOLDER_NAME, LAST_FOUR_DIGITS, EXPIRY_DATE, CARD_NUMBER FROM TBL_CARDS ";


        public Card()
        {
            Id = "";
            UserId = "";
            HolderName = "";
            LastFourDigits = "";
            ExpiryDate = "";
            CardNumber = "";
        }

        public Card(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            UserId = reader[reader.GetOrdinal("USER_ID")].ToString() ?? "";
            HolderName = reader[reader.GetOrdinal("HOLDER_NAME")].ToString() ?? "";
            LastFourDigits = reader[reader.GetOrdinal("LAST_FOUR_DIGITS")].ToString() ?? "";
            ExpiryDate = reader[reader.GetOrdinal("EXPIRY_DATE")].ToString() ?? "";
            CardNumber = reader[reader.GetOrdinal("CARD_NUMBER")].ToString() ?? "";
        }


        public Card(string userId, string holderName, string expiryDate, string cardNumber, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            UserId = userId;
            HolderName = holderName;
            LastFourDigits = cardNumber.Substring(cardNumber.Length - 4);
            ExpiryDate = expiryDate;
            CardNumber = cardNumber;
        }

        public static List<Card> GetCardsList()
        {
            try
            {
                var cards = new List<Card>();
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery);
                while (reader?.Read() ?? false)
                {
                    Card card = new Card(reader);
                    cards.Add(card);
                }
                return cards;
            }
            catch (Exception) { throw; }
        }

        internal static List<Card> GetByUserId(string userId)
        {
            string whereString = $" WHERE USER_ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, userId);
            List<Card> cards = new List<Card>();
            while (reader?.Read() ?? false)
            {
                Card card = new Card(reader);
                cards.Add(card);
            }
            reader?.Close();
            return cards;
        }

        internal int Insert()
        {
            string query = "Insert into TBL_CARDS (ID, USER_ID, HOLDER_NAME, LAST_FOUR_DIGITS, EXPIRY_DATE, CARD_NUMBER) VALUES (@0, @1, @2, @3, @4, @5)";
            int result = DatabaseConnection.NonQuery(query, Id, UserId, HolderName, LastFourDigits, ExpiryDate, SecurePassword.Hash(CardNumber));
            return result;
        }

        internal static int Delete(string id, string userId)
        {
            string query = "DELETE FROM TBL_CARDS WHERE ID = @0 AND USERID = @1";
            int result = DatabaseConnection.NonQuery(query, id, userId);
            return result;
        }

        internal static Card? GetById(string cardId)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, cardId);
            if (reader?.Read() ?? false)
            {
                Card card = new Card(reader);
                return card;
            }
            reader?.Close();
            return null;
        }

        internal int Delete()
        {
            string query = "DELETE FROM TBL_CARDS WHERE ID = @0";
            return DatabaseConnection.NonQuery(query, Id);
        }

        internal static Card? GetByCardNumberHash(string cardNumber)
        {
            string whereString = $" WHERE CARD_NUMBER = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, cardNumber);
            if (reader?.Read() ?? false)
            {
                Card card = new Card(reader);
                return card;
            }
            reader?.Close();
            return null;
        }
    }
}

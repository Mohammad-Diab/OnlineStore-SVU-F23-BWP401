using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore
{
    public class Purchase
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PurchaserName{ get; set; }
        public string PurchaserPhone { get; set; }
        public string PurchaserEmail { get; set; }
        public string PurchaserAddress { get; set; }
        public string DeleveryInstruction { get; set; }
        public string PaymentMethod { get; set; }
        public string CardId { get; set; }
        public string CartId { get; set; }
        public string State { get; set; }
        
        static string BaseQuery = "SELECT ID, USER_ID, PURCHASER_NAME, PURCHASER_EMAIL, PURCHASER_PHONE, PURCHASER_ADDRESS, DELIVERY_INSTRUCTION, PAYMENT_METHOD, CARD_ID, CART_ID, STATE FROM TBL_PURCHASE ";

        public Purchase()
        {
            Id = "";
            UserId = "";
            PurchaserName = "";
            PurchaserEmail = "";
            PurchaserPhone = "";
            PurchaserAddress = "";
            DeleveryInstruction = "";
            PaymentMethod = "";
            CardId = "";
            CartId = "";
            State = "";
        }

        public Purchase(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            UserId = reader[reader.GetOrdinal("USER_ID")].ToString() ?? "";
            PurchaserName = reader[reader.GetOrdinal("PURCHASER_NAME")].ToString() ?? "";
            PurchaserPhone = reader[reader.GetOrdinal("PURCHASER_PHONE")].ToString() ?? "";
            PurchaserEmail = reader[reader.GetOrdinal("PURCHASER_EMAIL")].ToString() ?? "";
            PurchaserAddress = reader[reader.GetOrdinal("PURCHASER_ADDRESS")].ToString() ?? "";
            DeleveryInstruction = reader[reader.GetOrdinal("DELIVERY_INSTRUCTION")].ToString() ?? "";
            PaymentMethod = reader[reader.GetOrdinal("PAYMENT_METHOD")].ToString() ?? "";
            CardId = reader[reader.GetOrdinal("CARD_ID")].ToString() ?? "";
            CartId = reader[reader.GetOrdinal("CART_ID")].ToString() ?? "";
            State = reader[reader.GetOrdinal("STATE")].ToString() ?? "";
        }


        public Purchase(string userId, string purchaseName, string purchasePhone, string purchaseEmail, string purchaseAddress, string deleveryInstructions, string paymentMethod, string cardId, string cartId, string state, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            UserId = userId;
            PurchaserName = purchaseName;
            PurchaserPhone = purchasePhone;
            PurchaserEmail = purchaseEmail;
            PurchaserAddress = purchaseAddress;
            DeleveryInstruction = deleveryInstructions;
            PaymentMethod = paymentMethod;
            CardId = cardId;
            CartId = cartId;
            State = state;
        }

        internal int Insert()
        {
            string query = "Insert into TBL_PURCHASE (ID, USER_ID, PURCHASER_NAME, PURCHASER_Phone, PURCHASER_EMAIL, PURCHASER_ADDRESS, DELIVERY_INSTRUCTION, PAYMENT_METHOD, CARD_ID, CART_ID, STATE) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10) ";
            int result = DatabaseConnection.NonQuery(query, Id, UserId, PurchaserName, PurchaserPhone, PurchaserEmail, PurchaserAddress, DeleveryInstruction, PaymentMethod, PaymentMethod == "cash" || string.IsNullOrEmpty(CardId) ? DBNull.Value : CardId, CartId, State);
            return result;
        }

        internal static int Delete(string id)
        {
            string query = "DELETE FROM TBL_PURCHASE WHERE ID = @0 ";
            int result = DatabaseConnection.NonQuery(query, id);
            return result;
        }

        internal static Purchase? GetById(string purchaseId)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, purchaseId);
            if (reader?.Read() ?? false)
            {
                Purchase purchase = new Purchase(reader);
                return purchase;
            }
            reader?.Close();
            return null;
        }

        internal int Delete()
        {
            string query = "DELETE FROM TBL_PURCHASE WHERE ID = @0 ";
            return DatabaseConnection.NonQuery(query, Id);
        }

        internal static Purchase? GetByUserId(string userId)
        {
            string whereString = $" WHERE USER_ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, userId);
            if (reader?.Read() ?? false)
            {
                Purchase purchase = new Purchase(reader);
                return purchase;
            }
            reader?.Close();
            return null;
        }
    }
}

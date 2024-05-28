using System.Data.SqlClient;

namespace OnlineStore
{
    public class CartProduct
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        static string BaseQuery = "SELECT ID, CART_ID, PRODUCT_ID, QUANTITY FROM TBL_CART_PRODUCTS ";

        public CartProduct()
        {
            Id = "";
            CartId = "";
            ProductId = "";
            Quantity = 0;
        }

        public CartProduct(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            CartId = reader[reader.GetOrdinal("CART_ID")].ToString() ?? "";
            ProductId = reader[reader.GetOrdinal("PRODUCT_ID")].ToString() ?? "";
            Quantity = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("QUANTITY")]);
        }

        public static List<CartProduct> GetCartProductsList(string cartId)
        {
            try
            {
                var cartProducts = new List<CartProduct>();
                string whereStatment = " WHERE CART_ID = @0 ";
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereStatment, cartId);
                while (reader?.Read() ?? false)
                {
                    CartProduct product = new CartProduct(reader);
                    cartProducts.Add(product);
                }
                reader?.Close();
                return cartProducts;
            }
            catch (Exception) { throw; }
        }

        public static CartProduct? GetById(string id)
        {
            string whereString = $" WHERE ID = @0 ";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, id);
            if (reader?.Read() ?? false)
            {
                CartProduct product = new CartProduct(reader);
                reader.Close();
                return product;
            }
            return null;
        }

        public CartProduct(string cartId, string productId, int quantity, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }

        internal int UpdateQuantity(int newQuantity)
        {
            if (newQuantity > 0)
            {
                Quantity = newQuantity;
                return Update();
            }
            else
                return Delete();
        }

        internal int Update()
        {
            string query = "UPDATE TBL_CART_PRODUCTS SET QUANTITY = @0 WHERE ID = @1";
            return DatabaseConnection.NonQuery(query, Quantity.ToString(), Id);
        }

        internal int Delete()
        {
            string query = "DELETE FROM TBL_CART_PRODUCTS WHERE ID = @0";
            return DatabaseConnection.NonQuery(query, Id);
        }

        internal int Insert()
        {
            string query = "Insert into TBL_CART_PRODUCTS (ID, CART_ID, PRODUCT_ID, QUANTITY) VALUES (@0, @1, @2, @3)";
            int result = DatabaseConnection.NonQuery(query, Id, CartId, ProductId, Quantity.ToString());
            return result;
        }
    }
}

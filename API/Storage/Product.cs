using System.Data.SqlClient;

namespace OnlineStore
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Image { get; set; }
        public int Warranty { get; set; }
        public long Price { get; set; }
        public int PurchaseCount { get; set; }
        public int AvailableStock { get; set; }

        static string BaseQuery = "SELECT ID, NAME, DESCRIPTION, USER_ID, IMAGE, WARRANTY, PRICE, PURCHASE_COUNT, AVAILABLE_STOCK FROM TBL_PRODUCTS ";

        public Product()
        {
            Id = "";
            Name = "";
            Description = "";
            UserId = "";
            Image = "";
            Warranty = 0;
            Price = 0;
            PurchaseCount = 0;
            AvailableStock = 0;
        }

        public Product(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            Name = reader[reader.GetOrdinal("NAME")].ToString() ?? "";
            Description = reader[reader.GetOrdinal("DESCRIPTION")].ToString() ?? "";
            UserId = reader[reader.GetOrdinal("USER_ID")].ToString() ?? "";
            Image = reader[reader.GetOrdinal("IMAGE")].ToString() ?? "";
            Warranty = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("WARRANTY")]);
            Price = DatabaseConnection.ConvertToInt64(reader[reader.GetOrdinal("PRICE")]);
            PurchaseCount = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("PURCHASE_COUNT")]);
            AvailableStock = DatabaseConnection.ConvertToInt32(reader[reader.GetOrdinal("AVAILABLE_STOCK")]);
        }

        internal static int Update(Product newValues)
        {
            string query = "UPDATE TBL_PRODUCTS SET NAME = @0, DESCRIPTION = @1, WARRANTY = @2, PRICE = @3, PURCHASE_COUNT = @4, AVAILABLE_STOCK = @5 WHERE ID = @6";
            return DatabaseConnection.NonQuery(query, newValues.Name, newValues.Description, newValues.Warranty.ToString(), newValues.Price.ToString(), newValues.PurchaseCount.ToString(), newValues.AvailableStock.ToString(), newValues.Id);
        }

        public Product(string name, string description, string userId,  int warranty, long price, int availableStock, string image = "", string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Name = name;
            Description = description;
            UserId = userId;
            Image = string.IsNullOrEmpty(image) ? Id : image;
            Warranty = warranty;
            Price = price;
            AvailableStock = availableStock;
            PurchaseCount = 0;
        }

        public static List<Product> GetProductsList(string userId = "")
        {
            try
            {
                var products = new List<Product>();
                string whereStatment = string.IsNullOrEmpty(userId) ? "" : " WHERE USER_ID = @0 ";
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereStatment, userId);
                while (reader?.Read() ?? false)
                {
                    Product product = new Product(reader);
                    products.Add(product);
                }
                reader?.Close();
                return products;
            }
            catch (Exception) { throw; }
        }

        public static List<Product> GetTodayDeals(int count)
        {
            try
            {
                var products = new List<Product>();
                string query = $"{BaseQuery} ORDER BY NEWID();";
                SqlDataReader? reader = DatabaseConnection.Query(query);
                int i = count;
                while (reader?.Read() ?? false && i > 0)
                {
                    Product product = new Product(reader);
                    products.Add(product);
                    i--;
                }
                reader?.Close();
                return products;
            }
            catch (Exception) { throw; }
        }

        public static List<Product> GetTopProducts(int count)
        {
            try
            {
                var products = new List<Product>();
                string query = $"{BaseQuery} ORDER BY PURCHASE_COUNT DESC";
                SqlDataReader? reader = DatabaseConnection.Query(query);
                int i = count;
                while (reader?.Read() ?? false && i > 0)
                {
                    Product product = new Product(reader);
                    products.Add(product);
                    i--;
                }
                reader?.Close();
                return products;
            }
            catch (Exception) { throw; }
        }

        internal static Product? GetById(string id)
        {
            string whereString = $" WHERE ID = @0";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, id);
            if (reader?.Read() ?? false)
            {
                Product product = new Product(reader);
                reader.Close();
                return product;
            }
            return null;
        }

        internal static bool UpdatePurchaseCount(string id, int updateBy)
        {
            Product? product = GetById(id);
            if (product != null)
            {
                product.PurchaseCount+= updateBy;
                Update(product);
                return true;
            }
            return false;
        }


        internal static bool UpdateAvailableStock(string id, int updateBy)
        {
            Product? product = GetById(id);
            if (product != null)
            {
                product.AvailableStock += updateBy;
                Update(product);
                return true;
            }
            return false;
        }

        internal int Insert()
        {
            string query = "Insert into TBL_PRODUCTS (ID, NAME, DESCRIPTION, USER_ID, WARRANTY, PRICE, PURCHASE_COUNT, IMAGE, AVAILABLE_STOCK) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
            int result = DatabaseConnection.NonQuery(query, Id, Name, Description, UserId, Warranty.ToString(), Price.ToString(), PurchaseCount.ToString(), Image, AvailableStock.ToString());
            return result;
        }

        internal int Delete(string userId)
        {
            string query = "Delete from TBL_PRODUCTS WHERE ID = @0 and USER_ID = @1;";
            int result = DatabaseConnection.NonQuery(query, Id, userId);
            return result;
        }
    }
}

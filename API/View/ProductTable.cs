namespace OnlineStore
{
    public class ProductTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Warranty { get; set; }
        public long Price { get; set; }
        public int AvailableStock { get; set; }
        public int PurchaseCount { get; set; }
        public IFormFile? Image { get; set; }

        public ProductTable(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Warranty = product.Warranty;
            Price = product.Price;
            AvailableStock = product.AvailableStock;
            PurchaseCount = product.PurchaseCount;
        }

        public ProductTable()
        {
            Name = string.Empty;
            Description = string.Empty;
            Warranty = 0;
            Price = 0;
            AvailableStock = 0;
            PurchaseCount = 0;
            Id = "";
        }

        public ProductTable(string id, string name, string description, int warranty, long price, int availableStock, int purchaseCount)
        {
            Id = id;
            Name = name;
            Description = description; 
            Warranty = warranty;
            Price = price;
            AvailableStock = availableStock;
            PurchaseCount = purchaseCount;
        }
    }
}

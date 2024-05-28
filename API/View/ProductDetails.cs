namespace OnlineStore
{
    public class ProductDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Seller { get; set; }
        public string Warranty { get; set; }
        public string Price { get; set; }
        public long AvailableStock { get; set; }
        public long PurchaseCount { get; set; }
        public string Image { get; set; }

        public ProductDetails(Product product, string sellerName, long purchaseCount)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Seller = sellerName;
            Warranty = Helper.ConvertDays(product.Warranty);
            AvailableStock = product.AvailableStock - purchaseCount;
            Price = $"{product.Price} SYP";
            PurchaseCount = purchaseCount;
            Image = product.Image;
        }
    }
}

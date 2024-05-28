namespace OnlineStore
{
    public class ProductCartTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long Amount { get; set; }
        public int AvailableStock { get; set; }

        public ProductCartTable(CartProduct cartProduct, Product product)
        {
            Id = cartProduct.Id;
            Name = product.Name;
            Quantity = cartProduct.Quantity;
            Price = product.Price;
            Amount = product.Price * cartProduct.Quantity;
            AvailableStock = product.AvailableStock;
        }
    }
}

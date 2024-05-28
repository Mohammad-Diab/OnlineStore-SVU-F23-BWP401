namespace OnlineStore
{
    public class ProductGrid
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }

        public ProductGrid(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Image = product.Image;
        }

        public ProductGrid()
        {
            Name = string.Empty;
            Price = 0;
            Id = "";
            Image = "";
        }

        public ProductGrid(string id, string name, long price, string image)
        {
            Id = id;
            Name = name;
            Price = price;
            Image = image;
        }
    }
}

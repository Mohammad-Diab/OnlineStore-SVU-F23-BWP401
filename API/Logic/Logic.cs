using System.Text.Json;

namespace OnlineStore
{
    public static class Logic
    {
        public static bool CreateUser(string email, string username, string password, string fullName, string phoneNumber)
        {
            var isExists = User.IsExists(username);
            if (!isExists)
            {
                User newUser = new User(username, fullName, email, password, phoneNumber);
                return newUser.Insert() == 1;
            }
            else
            {
                throw new Exception("المستخدم موجود بالفعل");
            }
        }

        internal static bool CreateProduct(string userId, ProductTable product, byte[] imageData, string fileEtension)
        {
            string pId = Guid.NewGuid().ToString();
            Product p = new Product(product.Name, product.Description, userId, product.Warranty, product.Price, product.AvailableStock, pId + fileEtension, pId);
            var result = p.Insert();
            if (result == 1)
            {
                return Files.StoreImage(imageData, p.Image);
            }
            return false;
        }

        internal static bool DeleteProduct(string id, string userId)
        {
            Product? p = Product.GetById(id);
            if (p == null)
            {
                throw new Exception("لم يتم العثور على المنتج المحدد");
            }
            if (userId == p.UserId)
            {
                return p.Delete(userId) > 0;
            }
            throw new Exception("المنتج لم تتم اضافته بواسطة المستخدم الحالي");
        }

        internal static List<UserTable> GetAllUsers()
        {
            List<User> users = User.GetUsersList();
            return users.Select(x => new UserTable(x)).ToList();
        }

        internal static List<ProductTable> GetMyProducts(string userId)
        {
            if (userId != null)
            {
                List<Product> products = Product.GetProductsList(userId);
                return products.Select(x => new ProductTable(x)).ToList();
            }
            return new List<ProductTable>();
        }

        internal static List<ProductGrid> GetTodayDeals()
        {

            List<Product> products = Product.GetTodayDeals(10);
            return products.Select(x => new ProductGrid(x)).ToList();
        }

        internal static List<ProductGrid> GetTopProducts()
        {
            List<Product> products = Product.GetTopProducts(10);
            return products.Select(x => new ProductGrid(x)).ToList();

        }

        internal static string GetImage(string imageName)
        {
            var imageBytes = Files.GetImage(imageName);
            return $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
        }

        internal static ProductDetails GetProduct(string userId, string productId)
        {
            Product? p = Product.GetById(productId);
            if (p == null) throw new Exception("لم يتم العثور على المنتج");
            string userName = User.GetByUserId(p.UserId)?.FullName ?? "مستخدم محذوف";
            Cart? cart = Cart.GetByUserId(userId);
            long purchaseCount = 0;
            if (cart != null)
            {
                cart.LoadProducts();
                purchaseCount = cart.Products.Find(x => x.ProductId == productId)?.Quantity ?? 0;
            }
            ProductDetails result = new ProductDetails(p, userName, purchaseCount);
            return result;
        }

        internal static List<ProductGrid> GetProducts()
        {
            List<Product> products = Product.GetProductsList();
            return products.Select(x => new ProductGrid(x)).ToList();
        }

        internal static bool AddToCart(string userId, string productId, int quantity)
        {
            Cart cart = Cart.GetOrCreate(userId);
            return cart.InsertProduct(productId, quantity) > 0;
        }

        internal static List<ProductCartTable> GetCart(string userId)
        {
            Cart? cart = Cart.GetByUserId(userId);
            List<Product> products = Product.GetProductsList();
            if (cart != null)
            {
                cart.LoadProducts();
                return (from CartProduct cartProduct in cart.Products
                        join Product product in products on cartProduct.ProductId equals product.Id
                        select new ProductCartTable(cartProduct, product)).ToList();
            }
            return new();
        }

        internal static bool ChangeQantity(string productCartId, int quantity)
        {
            CartProduct? cartProduct = CartProduct.GetById(productCartId);
            if (cartProduct == null)
            {
                throw new Exception("لم يتم العثور على العنصر");
            }
            return cartProduct.UpdateQuantity(quantity) > 0;
        }

        internal static bool DeleteFromCart(string productCartId)
        {
            CartProduct? cartProduct = CartProduct.GetById(productCartId);
            if (cartProduct == null)
            {
                throw new Exception("لم يتم العثور على العنصر");
            }
            return cartProduct.Delete() > 0;
        }

        internal static List<CardTable> GetCard(string userId)
        {
            var cards = Card.GetByUserId(userId);
            return cards.Select(x => new CardTable(x)).ToList();
        }

        internal static bool DeleteCard(string cardId)
        {
            Card? card = Card.GetById(cardId);
            if (card == null)
            {
                throw new Exception("لم يتم العثور على البطاقة");
            }
            return card.Delete() > 0;
        }

        internal static bool AddCard(string userId, CardRequest card)
        {
            Card newCard = new Card(userId, card.HolderName, card.ExpiryDate, card.Number);
            if (Card.GetCardsList().Any(x => SecurePassword.Verify(newCard.CardNumber, x.CardNumber)))
            {
                throw new Exception("يوجد بالفعل بطاقة بنفس الرقم يرجى ازالتها أولا ثم حاول مجددا");
            }
            return newCard.Insert() > 0;
        }

        internal static bool BuyNow(string userId, DeleveryRequest delevery)
        {
            Cart? cart = Cart.GetByUserId(userId);
            if (cart != null)
            {
                Purchase purchase = new Purchase(userId, delevery.ReciverName, delevery.Phone, delevery.Email, delevery.Address, delevery.Instructions, delevery.PayMethod, delevery.CardId, cart.Id, "open");
                if (cart.Closed) throw new Exception("لقد تم اغلاق هذه العربة مسبقا");
                var result = purchase.Insert();
                if (result > 0)
                {
                    cart.LoadProducts();
                    result = 0;
                    cart.Products.ForEach(x =>
                    {
                        result += Product.UpdateAvailableStock(x.ProductId, -x.Quantity) ? 1 : 0;
                        result += Product.UpdatePurchaseCount(x.ProductId, x.Quantity) ? 1 : 0;
                    });
                    if (result == cart.Products.Count * 2)
                    {
                        result = cart.Close();
                        return result == 1;
                    }
                    else
                    {
                        throw new Exception("خطأ أثناء القيام بعملية الشراء");
                    }
                }
                else
                {
                    throw new Exception("خطأ أثناء القيام بعملية الشراء");
                }

            }
            return true;
        }
    }
}

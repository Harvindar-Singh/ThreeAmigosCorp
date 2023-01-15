namespace Customer.Web.Product.Services
{
    public class FakeProductServices : IProductServices
    {
        private readonly ProductDto[] _products =
        {
           new ProductDto { ProductId= 1, BrandId = 1, ProductName = "Rippled Screen Protector",  CategoryName = "Screen Protectors", CategoryId = 1, Description = "For his or her sensory pleasure. Fits few known smartphones.", BrandName = "iStuff-R-Us", InStock = true, Price = 8.77 },
           new ProductDto { ProductId= 2, BrandId = 3, ProductName = "Wrap It and Hope Cover",  CategoryName = "Covers", CategoryId = 2, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", BrandName = "Soggy Sponge", InStock = true, Price = 3.46 },
           new ProductDto { ProductId= 3, BrandId = 2, ProductName = "Chocolate Cover",  CategoryName = "Screen Protectors", CategoryId = 2, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", BrandName = "Soggy Sponge", InStock = false, Price = 8.77 },
           new ProductDto { ProductId= 4, BrandId = 3, ProductName = "Water Bath Case",  CategoryName = "Case", CategoryId = 3, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", BrandName = "Soggy Sponge", InStock = true, Price = 11.46 },
           new ProductDto { ProductId= 5, BrandId = 3, ProductName = "Smartphone Car Holder",  CategoryName = "Accessories", CategoryId = 4, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", BrandName = "iStuff-R-Us", InStock = true, Price = 90.82 },
           new ProductDto { ProductId= 6, BrandId = 3, ProductName = "Sticky Tape Sport Armband",  CategoryName = "Accessories", CategoryId = 4, Description = "Keep your device on your arm with this general purpose sticky tape.", BrandName = "Soggy Sponge", InStock = true, Price = 2.23 },
           new ProductDto { ProductId= 7, BrandId = 1, ProductName = "Cloth Cover",  CategoryName = "Covers", CategoryId = 2, Description = "Lamely adapted used and dirty teatowel. Guaranteed fewer than two holes.", BrandName = "iStuff-R-Us", InStock = true, Price = 2.9 }

        };

        public Task<ProductDto> GetProductAsync(int id)
        {
            var product = _products.FirstOrDefault(r => r.ProductId == id);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<ProductDto>> GetProductsAsync(string product)
        {
            var products = _products.AsEnumerable();
            if (product != null)
            {
                products = products.Where(r => r.ProductName.Equals(product, StringComparison.OrdinalIgnoreCase));
            }
            return Task.FromResult(products);
        }



    }
}


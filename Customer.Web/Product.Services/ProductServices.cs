using System.Net;

namespace Customer.Web.Product.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient _client;

        public ProductServices(HttpClient client)

        {
            client.BaseAddress = new System.Uri("http://localhost:3733/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var response = await _client.GetAsync("api/products/" + id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsAsync<ProductDto>();
            return product;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(string productsname)
        {
            var uri = "api/products?category=MOV";
            if (productsname != null)
            {
                uri = uri + "&productsname=" + productsname;
            }
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            return products;
        }



    }
}

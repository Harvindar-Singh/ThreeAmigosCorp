using System;

namespace Customer.Web.Product.Services
{
    public interface IProductServices
    {
        public Task<IEnumerable<ProductDto>> GetProductsAsync(string productname);

        public Task<ProductDto> GetProductAsync(int Id);

    }
}

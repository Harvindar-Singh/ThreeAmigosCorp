using System;

namespace Customer.Web.Product.Services
{
    public interface IProductServices
    {
        public Task<IEnumerable<ProductDto>> GetProductsAsync();

        public Task<ProductDto> GetProductAsync(int Id);

    }
}

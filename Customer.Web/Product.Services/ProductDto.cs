using System.ComponentModel.DataAnnotations;

namespace Customer.Web.Product.Services
{
    public class ProductDto
    {
        [Key]
        public int ProductId { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }


        public string BrandName { get; set; }


        public bool InStock { get; set; }

        public double Price { get; set; }

    }
}

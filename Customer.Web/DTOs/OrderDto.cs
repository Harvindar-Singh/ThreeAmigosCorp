using System.ComponentModel.DataAnnotations;

namespace Customer.Web.DTOs
{
    public class OrderDto
    {
        [Key]
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        public string EmailAddress { get; set; }

        public string DeliveryAddress { get; set; }

        public string Description { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Customer.Web.DTOs
{
    public class ReviewDto
    {

        [Key]
        public int ReviewId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }
    }
}

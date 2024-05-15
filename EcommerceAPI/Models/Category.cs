using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "Last Name Should be more than 3 letters")]
        public string Title { get; set; }
        // Navigation Property
        public virtual List<Product> Products { get; set; }
    }
}

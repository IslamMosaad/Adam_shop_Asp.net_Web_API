using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EcommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage = "First Name Should be more than 3 letters")]
        public string Title { get; set; }
        [MinLength(20, ErrorMessage = "First Name Should be more than 20 letters")]
        public string Description { get; set; }
        public string Img { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<CartItem> Cart { get; set; }


    }
}

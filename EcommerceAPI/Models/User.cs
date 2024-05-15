using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace EcommerceAPI.Models
{
    public class User : IdentityUser
    {
        //public string Id { get; set; }
        [MinLength(3, ErrorMessage = "First Name Should be more than 3 letters")]
        public string FirstName { get; set; }
        [MinLength(3, ErrorMessage = "Last Name Should be more than 3 letters")]
        public string Lastname { get; set; }
        public string? ProfileImage { get; set; }
        [RegularExpression("Male | Female",ErrorMessage ="Gender Should be Male Or Female")]
        public string Gender { get; set; }
        public string Address { get; set; }
        public string? Visa { get; set; }

        // Navigation Property
        public virtual List<Product> Products { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Payment> payments { get; set; }
        public virtual List<Order> orders { get; set; }
        public virtual List<CartItem> Cart { get; set; }
    }
}

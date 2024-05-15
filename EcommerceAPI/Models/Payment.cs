using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public decimal TotalPrice { get; internal set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual List<Order> orders { get; set; }=new List<Order>();
    }
}

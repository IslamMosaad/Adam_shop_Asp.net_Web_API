using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        //[ForeignKey("CartItem")]
        //public int? CartItemId { get; set; }

        // Navigation Properties
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        //public virtual CartItem CartItem { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("user")]
        public string UserID { get; set; }
        [ForeignKey("product")]
        public int ProductID { get; set; }
        //[ForeignKey("OrderItem")]
        //public int? OrderItemId { get; set; }
        public int Quantity { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        //public virtual OrderItem OrderItem { get; set; }
    }
}

namespace EcommerceAPI.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Percentage { get; set; } = 0;
        public virtual List<Order> Orders { get; set; }= new List<Order>();
    }
}

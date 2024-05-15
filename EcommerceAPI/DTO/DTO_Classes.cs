using EcommerceAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.DTO
{
    public class ProductDTO
    {
        public int id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string img { get; set; }

        public decimal price { get; set; }

        public int stock { get; set; }

        public string? userID { get; set; }

        public int? categoryId { get; set; }
        public string? category_name { get; set; }
    }

    public class CategoryDTO
    {
        public int id { get; set; }
        [MinLength(3, ErrorMessage = "Last Name Should be more than 3 letters")]
        public string title { get; set; }

    }

    public class CommentDTO
    {
        public int id { get; set; }
        public string body { get; set; }
        public int product_id { get; set; }
        public string user_id { get; set; }
        public string? product_name { get; set; }
    }

    public class OrderDTO
    {
        public int id { set; get; }
        public DateTime date { set; get; }
        public decimal price { set; get; }
        public string userId { set; get; }
        public string? user_name { set; get; }
        public int paymentId { set; get; }
        public int? promoCodeId { set; get; }
        public int? promoCode_name { set; get; }
    }

    public class OrderItemDTO
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int productId { get; set; }
        public string? product_name { get; set; }
        public int? product_price { get; set; }
        public int orderId { get; set; }
   
    }


    public class CartItemDTO
    {
        public int? id { get; set; }
        public string? user_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }

        public string? user_name { set; get; }
        public string? product_title { get; set; }
        public string? product_img { get; set; }
        public decimal? product_price { get; set; }
    }

    public class PaymentDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public decimal totalPrice { get; internal set; }
        public string userId { get; set; }

        public string? user_name { set; get; }
    }


}

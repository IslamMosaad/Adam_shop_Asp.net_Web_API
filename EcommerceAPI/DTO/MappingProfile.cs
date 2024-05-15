using AutoMapper;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region from Product to ProductDTO
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.img, opt => opt.MapFrom(src => src.Img))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.stock, opt => opt.MapFrom(src => src.Stock))
            .ForMember(dest => dest.categoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.category_name, opt => opt.MapFrom(src => src.Category.Title))
            ;
            // Mapping from ProductDTO to Product (reverse direction)
            CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.img))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.stock))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.categoryId))
            ;

            #endregion

            #region from Category to CategoryDTO
            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
            ;
            // Mapping from CategoryDTO to Category (reverse direction)
            CreateMap<CategoryDTO, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title))
            ;

            #endregion


            #region from Order to OrderDTO
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.user_name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.Lastname}"))
            .ForMember(dest => dest.paymentId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(dest => dest.promoCodeId, opt => opt.MapFrom(src => src.PromoCodeId))
            .ForMember(dest => dest.promoCode_name, opt => opt.MapFrom(src => src.PromoCode.Code))
            ;
            // Mapping from OrderDTO to Order (reverse direction)
            CreateMap<OrderDTO, Order>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
             .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.date))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId))
             .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.paymentId))
             .ForMember(dest => dest.PromoCodeId, opt => opt.MapFrom(src => src.promoCodeId))
             ;
            #endregion


            #region from OrderItem to OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.product_name, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.product_price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.orderId, opt => opt.MapFrom(src => src.OrderId))
            ;
            // Mapping from OrderDTO to Order (reverse direction)
            CreateMap<OrderItemDTO, OrderItem>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.productId))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.orderId))
            ;
            #endregion


            #region from CartItem to CartItemDTO
            CreateMap<CartItem, CartItemDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.user_id, opt => opt.MapFrom(src => src.UserID))
            .ForMember(dest => dest.product_id, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.user_name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.Lastname}"))
            .ForMember(dest => dest.product_price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.product_title, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.product_img, opt => opt.MapFrom(src => src.Product.Img))
            ;
            // Mapping from CartItemDTO to CartItem (reverse direction)
            CreateMap<CartItemDTO, CartItem>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.user_id))
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.product_id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
            ;
            #endregion

            #region from Payment to PaymentDTO
            CreateMap<Payment, PaymentDTO>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.state, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.zip, opt => opt.MapFrom(src => src.Zip))
            .ForMember(dest => dest.country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.totalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.user_name, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.Lastname}"))
            ;
            // Mapping from PaymentDTO to Payment (reverse direction)
            CreateMap<PaymentDTO, Payment>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.state))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.zip))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.totalPrice))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId))
            ;
            #endregion
       

        }
    }

}

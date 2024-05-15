using EcommerceAPI.DTO;

namespace EcommerceAPI.Services
{
    public interface IUserService
    {
        Task<UserManagerResponseDTO> RegisterUserAsync(RegisterDTO model, bool isAdmin);

        Task<UserManagerResponseDTO> LoginUserAsync(LoginDTO model);
         Task<UserManagerResponseDTO> LogOutUserAsync();
    }
}

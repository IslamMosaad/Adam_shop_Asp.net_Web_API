using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EcommerceAPI.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration configuration;


        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }



        public async Task<UserManagerResponseDTO> RegisterUserAsync(RegisterDTO model,bool isAdmin)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is null");

            if (model.Password == model.ConfirmPassword && model.FirstName != null && model.Lastname != null && model.Password != null && model.ConfirmPassword != null &&
                model.PhoneNumber != null && model.Gender != null && model.Address != null)
            {

                User user = new User
                {
                    FirstName = model.FirstName,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Visa = model.Visa,
                    ProfileImage = model.ProfileImage,
                    Gender = model.Gender,
                    Address = model.Address,
                    UserName = model.FirstName + model.Lastname + Guid.NewGuid().ToString()
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create Role
                    string roleName = (isAdmin) ? "Admin" : "NormalUser";
                    //IdentityRole roleModel = new IdentityRole() { Name = roleName };
                   // IdentityResult rust = await roleManager.CreateAsync(roleModel);
                    //Assign Role to User
                    IdentityResult resultRole = await userManager.AddToRoleAsync(user, roleName);


                    await signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    return new UserManagerResponseDTO
                    {
                        Message = "User did not create",
                        IsSuccess = false,
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }


                return new UserManagerResponseDTO()
                {
                    Message = "User registered successfully",
                    IsSuccess = true,
                };
            }
            else
            {
                return new UserManagerResponseDTO()
                {
                    Message = "Confirm password doesn't match the password or you forget required input",
                    IsSuccess = false,
                };
            }
        }



        public async Task<UserManagerResponseDTO> LoginUserAsync(LoginDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)//wrong password
            {
                return new UserManagerResponseDTO
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            }
            else //Right password
            {
                #region Generate Token Steps
                //1 secret key
                string key = configuration["AuthSettings:Key"] ?? "Adam Islam Mosad Adly the strongest 4 years old champion";
                var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                //2 
                var signingcer = new SigningCredentials(secertkey, SecurityAlgorithms.HmacSha256);

                //3 
                List<Claim> userdata = new List<Claim>();
                userdata.Add(new Claim("Email", model.Email));
                userdata.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                //4
                var token = new JwtSecurityToken(
                    claims: userdata,
                    expires: DateTime.Now.AddDays(30), //new DateTime(2025, 1, 25);
                    signingCredentials: signingcer
                    );

                //5
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
                #endregion

                var roles = await userManager.GetRolesAsync(user);
                bool isAdmin = roles.Contains("Admin");
                return new UserManagerResponseDTO
                {
                    Message = tokenstring,
                    IsAdmin = isAdmin,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };

            }
        }


        public async Task<UserManagerResponseDTO> LogOutUserAsync()
        {
            await signInManager.SignOutAsync();


            #region Generate Token Steps
            //1 secret key
            string key = configuration["AuthSettings:Key"] ?? "Adam Islam Mosad Adly the strongest 4 years old champion";
            var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            //2 
            var signingcer = new SigningCredentials(secertkey, SecurityAlgorithms.HmacSha256);

            //3 
            List<Claim> userdata = new List<Claim>();

            //4
            var token = new JwtSecurityToken(
                claims: userdata,
                expires: new DateTime(2020, 1, 25),
                signingCredentials: signingcer
                );

            //5
            var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
            #endregion

            return new UserManagerResponseDTO
            {
                Message = tokenstring,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };


        }





    }
}

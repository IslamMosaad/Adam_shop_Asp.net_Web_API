
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using EcommerceAPI.Unit_OF_Work;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EcommerceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region Allow CORS
            string txtToAllowCORS = "";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txtToAllowCORS,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            #endregion

            #region for Swagger Doc To Allow sending Token 
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Islam Mosad Ecommerce API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Here Enter JWT Token with bearer format like bearer [space] token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            #endregion


            #region register connection string for my database & using LazyLoading
            builder.Services.AddDbContext<ECommerceDBContext>(options =>
               options.UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("cs")));
            #endregion

            #region Adding IdentityUser & IdentityRole Model
            builder.Services.AddIdentity<User, IdentityRole>(
                                options =>
                                {
                                    options.User.RequireUniqueEmail = true;
                                    options.Password.RequireNonAlphanumeric = false;
                                    options.Password.RequireDigit = false;
                                    options.Password.RequireUppercase = false;
                                    options.Password.RequireLowercase = false;
                                    options.Password.RequiredLength = 8;
                                }).AddEntityFrameworkStores<ECommerceDBContext>().AddDefaultTokenProviders();
            #endregion

           

            #region register UnitOfWork & Configuration & myServices 
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddScoped<IUnitOfWork<Product>, UnitOfWork<Product>>();
            builder.Services.AddScoped<IUnitOfWork<CartItem>, UnitOfWork<CartItem>>();
            builder.Services.AddScoped<IUnitOfWork<Category>, UnitOfWork<Category>>();
            builder.Services.AddScoped<IUnitOfWork<Order>, UnitOfWork<Order>>();
            builder.Services.AddScoped<IUnitOfWork<Comment>, UnitOfWork<Comment>>();
            builder.Services.AddScoped<IUnitOfWork<OrderItem>, UnitOfWork<OrderItem>>();
            builder.Services.AddScoped<IUnitOfWork<Payment>, UnitOfWork<Payment>>();
            builder.Services.AddScoped<IUnitOfWork<User>, UnitOfWork<User>>();
            builder.Services.AddScoped<IUnitOfWork<PromoCode>, UnitOfWork<PromoCode>>();

            builder.Services.AddScoped<IProductCacheM, ProductCacheM>();

            #endregion

            //use autoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            #region using JWT Bearer Token for Authentication
            builder.Services.AddAuthentication(option =>
            {
                //option.DefaultAuthenticateScheme = "mySchema"; 
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//1

            }) //.AddJwtBearer("mySchema", op => {
              .AddJwtBearer(op =>
              {

                  string key = builder.Configuration["AuthSettings:Key"] ?? "Adam Islam Mosad Adly the strongest 4 years old champion";
                  var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                  op.TokenValidationParameters = new TokenValidationParameters
                  {
                      IssuerSigningKey = secertkey,
                      ValidateLifetime = true,
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateActor = false,
                      RequireExpirationTime = false,
                      ValidateIssuerSigningKey = false

                  };
              });
            #endregion


            #region cacheManagement Service
            builder.Services.AddMemoryCache();

            #endregion
            var app = builder.Build();






            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(txtToAllowCORS);
            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }



}

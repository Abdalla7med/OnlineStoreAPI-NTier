
using BLL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using System.Security.Cryptography;


namespace OnlineStoreAPI
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /// this's the solution for the problem JsonCycle  in Order.OrderDetails.Order
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                // Adjust depth as needed, this to helps ignore the cycle of navigational property
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
                            });

            /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            /// Add configuration from appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<OnlineStoreContext>( options =>
                                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            /// Register ApplicationUser and adding the Token verification 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                              .AddEntityFrameworkStores<OnlineStoreContext>()
                              .AddDefaultTokenProviders();

            var Issuer = builder.Configuration["Jwt:Issuer"];
            var Audience = builder.Configuration["Jwt:Audience"];

            /// Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                /// use default scheme to add all needed things 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience, /// From Json 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            /// Swagger Registration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineStoreAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer' followed by a space and your JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
            });

            /// Register All Repositories 
            /// this pattern doesn't recommended, so try to use productRepository it self

            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<CustomerRepository>();

            /// Register all Services, make sense
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<CustomerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineStoreAPI v2"); // for swagger documentation 
                });
            }

            /// Https redirect 
            app.UseHttpsRedirection();

            /// Authorization & Authentication
            app.UseAuthorization();

            app.UseAuthentication();  /// to enable Authorize Actions, default in cookie

            app.MapControllers(); 

            app.Run();
        }
    }
}

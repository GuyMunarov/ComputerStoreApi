
using System.Text;
using Infrastructure.DatabaseConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace Infrastructure.BootstrapingExtensions
{
    public static class BootstrapingExtensions
    {
        public static void AddInitialDependencies(this IServiceCollection services, IConfiguration config)
        {

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
            services.AddEndpointsApiExplorer();



            services.AddDbContext<DataContext>(x =>
                x.UseLazyLoadingProxies().UseSqlite(config.GetConnectionString("DefaultConnection")
                    , x => x.MigrationsAssembly("Infrastructure")
                ));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddStackExchangeRedisCache(options => { options.Configuration = config["RedisCacheUrl"]; });


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
        }
    }
}

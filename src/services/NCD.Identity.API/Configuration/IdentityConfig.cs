using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCD.Identity.API.Data;
using NCD.Identity.API.Extensions;
using NetDevPack.Security.Jwt;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace NCD.Identity.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppTokenSettings");
            services.Configure<AppTokenSettings>(appSettingsSection);

            services.AddJwksManager(options => options.Jws = JwsAlgorithm.ES256)
                    .PersistKeysToDatabaseStore<IdentityAppDbContext>();

            services.AddDbContext<IdentityAppDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityAppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}

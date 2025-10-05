using TalkFlow.Data;
using TalkFlow.Helpers;
using TalkFlow.Interfaces;
using TalkFlow.Services;
using TalkFlow.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TalkFlow.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigCORS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    // Allow localhost origins and configured origins
                    policy.SetIsOriginAllowed(origin =>
                    {
                        // Always allow localhost for development
                        if (origin.StartsWith("http://localhost") || origin.StartsWith("https://localhost"))
                            return true;
                            
                        // Check configured origins
                        var allowedOrigins = configuration.GetSection("OriginAllowed").Get<string[]>();
                        return allowedOrigins?.Contains(origin) ?? false;
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
        }

        public static void ConfigDatabase(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void ConfigRegister(this IServiceCollection services)
        {
            services.AddSingleton<PresenceTracker>();
            services.AddSingleton<StrangerTracker>();
            services.AddSingleton<UserShareScreenTracker>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<LogUserActivity>();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        }


    }
}



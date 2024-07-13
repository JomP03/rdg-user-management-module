using App.Services.Mappers;
using App.Services.Repositories.Shared;
using App.Services.Services.Auth;
using App.Services.Services.SignUpRequests;
using App.Services.Services.Skeletons;
using App.Services.Services.Users;
using IAM.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Repositories.Shared;
using System;

namespace Services.API.Extensions
{
    /// <summary>
    /// Extension for the dependency injections
    /// </summary>
    public static class LocalServicesExtensions
    {
        /// <summary>
        /// Adds the local services to the dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddLocalServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["userdbConString"];
            Console.WriteLine("Connection string: " + connectionString);
            services.AddDbContext<UMDatabaseContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            Console.WriteLine("Added DbContext");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISkeletonService, SkeletonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserGatewayFactory, UserGatewayFactory>();
            services.AddScoped<ISignUpRequestService, SignUpRequestService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<ISignUpRequestMapper, SignUpRequestMapper>();

        }
    }
}

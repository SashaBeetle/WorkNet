using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worknet.Shared.Interfaces;
using Worknet.Infrastructure.GoogleDrive;

namespace Worknet.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<GoogleDriveClient>();

            services.AddSingleton<IGoogleDriveService, GoogleDriveService>();

            return services;
        }
    }
}

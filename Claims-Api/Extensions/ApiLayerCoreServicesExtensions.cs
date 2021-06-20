using Claims_Api.Repositories;
using Claims_Api.Repositories.UnitOfWork;
using Claims_Api.Services.Claim;
using Claims_Api.Services.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Claims_Api.Extensions
{
    public static class ApiLayerCoreServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IQueueService, QueueService>();
        }
        public static void AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IClaimService, ClaimService>();
        }
    }
}
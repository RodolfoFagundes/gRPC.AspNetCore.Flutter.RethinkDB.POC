using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcServer
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddSingleton<Room>();
            services.AddSingleton<ChatRepository>();
            services.AddSingleton<IHostedService, ChatChangefeedBackgroundService>();
            return services;
        }
    }
}

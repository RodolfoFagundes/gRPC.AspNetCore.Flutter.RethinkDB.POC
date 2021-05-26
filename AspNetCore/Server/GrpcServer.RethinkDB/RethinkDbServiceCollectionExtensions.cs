using GrpcServer.Db.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GrpcServer.RethinkDB
{
    public static class RethinkDbServiceCollectionExtensions
    {
        public static IServiceCollection AddRethinkDb(this IServiceCollection services, Action<RethinkDbOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            services.TryAddSingleton<IRethinkDbSingletonProvider, RethinkDbSingletonProvider>();
            services.TryAddTransient<IChatChangefeedDbService, ChatRethinkDbService>();

            return services;
        }
    }
}

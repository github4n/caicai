using Microsoft.Extensions.DependencyInjection.Extensions;
using Smart.Core.Throttle;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiThrottle(this IServiceCollection services, Action<ApiThrottleOptions> options)
        {
            services.Configure(options);
            services.TryAddSingleton<IStorageProvider, RedisStorageProvider>();
            services.TryAddSingleton<ICacheProvider, RedisCacheProvider>();
            services.TryAddSingleton<IApiThrottleService, ApiThrottleService>();
            //Options and extension service
            var opts = new ApiThrottleOptions();
            options(opts);

            //foreach (var serviceExtension in opts.Extensions)
            //{
            //    serviceExtension.AddServices(services);
            //}
            services.AddSingleton(opts);
            return services;
        }
    }
}
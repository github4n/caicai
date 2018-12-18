using Lottery.Services;
using Lottery.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            {
                services.AddSingleton<IUsersService, UsersService>();
                services.AddSingleton<IBJDC_DataService, BJDC_DataService>();
            }

            return services;
        }
    }
}

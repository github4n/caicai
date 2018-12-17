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
            }

            return services;
        }
    }
}

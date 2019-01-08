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
                services.AddSingleton<ISport_DataService, Sport_DataService>();
                services.AddSingleton<IXML_DataService, XML_DataService>();
                services.AddSingleton<IApi_DataService, Api_DataService>();
                services.AddSingleton<IDigitalLotteryService, DigitalLotteryService>();
                services.AddSingleton<ILotteryDetailService, LotteryDetailService>();
                services.AddSingleton<IKaiJiangWangService,KaiJiangWangService>();
            }

            return services;
        }
    }
}

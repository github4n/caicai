//using Smart.Data.Dapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryOptionsExtensions
    {
        /// <summary>
        /// 使用Dapper持久化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        //public static IServiceCollection AddDataDapper(this IServiceCollection services, Action<RepositoryConfig> configure)
        //{
        //    var mysqlOptions = new RepositoryConfig();
        //    configure(mysqlOptions);
        //    services.AddSingleton(mysqlOptions);
        //    services.AddTransient(typeof(IDataManager), typeof(DataManager));
        //    return services;
        //}

    }
}

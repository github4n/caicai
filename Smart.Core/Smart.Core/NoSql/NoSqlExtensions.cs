using Smart.Core.NoSql.Mongodb;
using Smart.Core.NoSql.Redis;
using Smart.Core.Utils;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// nosql服务扩展
    /// </summary>
    public static class NoSqlExtensions
    {
        /// <summary>
        /// 使用Redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddCSRedis(
           this IServiceCollection services,
           Action<List<RedisConfig>> options = null)
        {
            List<RedisConfig> option = new List<RedisConfig>();
            options?.Invoke(option);
            //ObjectMapper.MapperTo<RedisConfig>(option, ConfigFileHelper.Get<RedisConfig>());//优先级装饰器
            services.AddSingleton(option);
            services.AddSingleton<RedisManager>();
            return services;
        }

        /// <summary>
        /// 使用Mongodb
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongodb(
          this IServiceCollection services,
          Action<MongodbConfig> options = null)
        {
            MongodbConfig option = new MongodbConfig();
            options?.Invoke(option);
            ObjectMapper.MapperTo<MongodbConfig>(option, ConfigFileHelper.Get<MongodbConfig>());//优先级装饰器
            services.AddSingleton(option);
            services.AddSingleton<MongodbManager>();
            return services;
        }
    }
}

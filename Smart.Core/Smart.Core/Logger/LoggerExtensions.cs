using Smart.Core.Logger;
using Smart.Core.Utils;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 使用文件日志
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileLogger(
            this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger), typeof(TextFileLogger));
            return services;
        }

        /// <summary>
        /// 使用log4日志
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddLog4Logger(
            this IServiceCollection services,
            Action<LoggerConfig> options)
        {
            var option = new LoggerConfig();
            options?.Invoke(option);
            ObjectMapper.MapperTo(option, ConfigFileHelper.Get<LoggerConfig>());
            services.AddSingleton(option);
            services.AddSingleton(typeof(ILogger), typeof(Log4Logger));
            return services;
        }

        /// <summary>
        /// 使用api日志
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsoleLogger(
            this IServiceCollection services,
            Action<LoggerConfig> options)
        {
            var option = new LoggerConfig();
            options?.Invoke(option);
            ObjectMapper.MapperTo(option, ConfigFileHelper.Get<LoggerConfig>());
            services.AddSingleton(option);
            services.AddSingleton(typeof(ILogger), typeof(ConsoleLogger));
            return services;
        }
    }
}
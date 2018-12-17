using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Smart.Core.Throttle;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Middleware扩展方法
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 请求响应时间
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StaticHttpContextMiddleware>();
        }

        /// <summary>
        /// Api限流
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiThrottle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiThrottleMiddleware>();
        }
    }
}

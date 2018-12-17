using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 静态的http请求中间件
    /// </summary>
    public class StaticHttpContextMiddleware
    {
        private readonly RequestDelegate _next;

        public StaticHttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var httpContextAccessor = context.RequestServices.GetService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// http当前上下文对象
    /// </summary>
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
}
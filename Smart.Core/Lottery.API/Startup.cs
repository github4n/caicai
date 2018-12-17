using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smart.Core.Repository.SqlSugar;
using Smart.Core.Throttle;
using Smart.Core.Utils;
using SqlSugar;
using Swashbuckle.AspNetCore.Swagger;

namespace Lottery.API
{
    /// <summary>
    /// orm框架 http://www.codeisbug.com/Doc/8
    /// csredis  https://github.com/2881099/csredis
    /// redis命令 http://www.redis.net.cn/order/3530.html
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration 属性
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options=> { options.Filters.Add(typeof(ApiThrottleActionFilter)); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ConfigFileHelper.Set("config.json");
            #region CORS
            //跨域第二种方法，声明策略，记得下边app中配置
            services.AddCors(c =>
            {
                //↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    .AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
                //↑↑↑↑↑↑↑注意正式环境不要使用这种全开放的处理↑↑↑↑↑↑↑↑↑↑


                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    .WithOrigins("http://127.0.0.1:1818", "http://localhost:8080", "http://localhost:8021", "http://localhost:8081", "http://localhost:1818")//支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });

            //跨域第一种办法，注意下边 Configure 中进行配置
            //services.AddCors();
            #endregion

            #region Swagger
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v0.1.0",
            //        Title = "Blog.Core API",
            //        Description = "框架说明文档",
            //        TermsOfService = "None",
            //        Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Blog.Core", Email = "Blog.Core@xxx.com", Url = "https://www.baidu.com" }
            //    });
            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Lottery.API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Lottery.API", Email = "Lottery.API@xxx.com", Url = "https://www.baidu.com" }
                });

                //就是这里
                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Lottery.API.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "Lottery.Modes.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Lottery.API", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("Lottery.API", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });
            #endregion

            #region Token服务注册
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            //});
            #endregion

            services.AddSqlSugarClient<DbFactory>((sp, op) =>
            {
                op.ConnectionString = ConfigFileHelper.Get("Lottery:Data:Database:Connection");
                op.DbType = DbType.MySql;
                op.IsAutoCloseConnection = false;
                op.InitKeyType = InitKeyType.Attribute;
                op.IsShardSameThread = true;
            });
            services.AddServices();
            services.AddCSRedis(options =>
            {
                options.Add(new Smart.Core.NoSql.Redis.RedisConfig() { C_IP = "127.0.0.1", C_Post = 6379, C_Password = "redis123", C_Defaultdatabase = 0 });
                options.Add(new Smart.Core.NoSql.Redis.RedisConfig() { C_IP = "127.0.0.1", C_Post = 6379, C_Password = "redis123", C_Defaultdatabase = 1 });
            });
            services.AddConsoleLogger(options => { });

            services.AddApiThrottle(options =>
            {
                options.Global.AddValves(new BlackListValve
                {
                    Policy = Policy.Ip,
                    Priority = 99
                }, new WhiteListValve
                {
                    Policy = Policy.UserIdentity,
                    Priority = 88
                },
               new BlackListValve
               {
                   Policy = Policy.Header,
                   PolicyKey = "throttle"
               });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    //c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件
                });
                #endregion
            }
            else
            {
                app.UseHsts();
            }
            //app.UseStaticHttpContext();
            //app.UseHttpsRedirection();
            //Api限流
            app.UseApiThrottle();
            app.UseMvc();
        }
    }
}

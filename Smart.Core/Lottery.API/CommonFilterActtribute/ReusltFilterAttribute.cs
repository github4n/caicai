using EntityModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Smart.Core.Encrypt.Easyman.Common.Helper;
using Smart.Core.Extensions;
using Smart.Core.Throttle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lottery.Api.Controllers.CommonFilterActtribute
{

    public class ReusltFilterAttribute : ActionFilterAttribute
    {
        //public string[] AllowSites { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var origin = context.HttpContext.Request.Headers["Origin"].ToString();
            string requestHeaders = context.HttpContext.Request.Headers["Access-Control-Request-Headers"];
            Action action = () =>
            {
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.HttpContext.Response.Headers.Add("Access-Control-Request-Headers", "Content-Type");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            };
            action();
            if (context.HttpContext.Request.Path.Value.ToLower() == "api/data/getnowdate")
            {
                base.OnActionExecuting(context);
            }
            else
            {
                try
                {
                    var t = context.HttpContext.Request.Form["TimeStamp"];
                    var dt = EncryptHelper.AesDecrpt(t);
                    var clientTime = DateTimeHelper.StampToDateTime(dt);
                    var now = DateTime.Now;
                    var sec = now.Subtract(clientTime).TotalSeconds;
                    double interval = 90;
                    if (interval < Math.Abs(sec))
                    {
                        var result = new LotteryServiceResponse()
                        {
                            Code = ResponseCode.TimeStampError,
                            Message = "请求发生异常"
                        };
                        context.Result = new JsonResult(result);
                    }
                    else
                    {
                        base.OnActionExecuting(context);
                    }
                }
                catch (Exception ex)
                {
                    var result = new LotteryServiceResponse()
                    {
                        Code = ResponseCode.TimeStampError,
                        Message = "请求发生异常"
                    };
                    context.Result = new JsonResult(result);
                }
            }
        }
    }
}

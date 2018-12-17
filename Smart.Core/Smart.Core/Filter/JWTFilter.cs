using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Smart.Core.JWT;
using Smart.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Filter
{
    public class JWTFilter : Attribute, IActionFilter
    {
        public string Policy { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            var user = new JWTHelper().GetUserInfo(token);
            if (!string.IsNullOrEmpty(user.Name) && user.Role == Policy)
            {

            }
            else
            {
                context.Result = new ObjectResult(Result.Fail(1012, "权限不足"));

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

    }
}

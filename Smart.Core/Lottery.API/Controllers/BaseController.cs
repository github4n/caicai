using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.JWT;

namespace Lottery.API.Controllers
{
    /// <summary>
    /// 公共
    /// </summary>
    public class BaseController : ControllerBase
    {

        /// <summary>
        /// 获取header token
        /// </summary>
        /// <returns></returns>
        public UserInfo GetHeaderToken
        {
            get
            {
                var authorization = HttpContext.Request.Headers["Authorization"];
                var user = new JWTHelper().GetUserInfo(authorization);
                return user;
            }
        }
    }
}
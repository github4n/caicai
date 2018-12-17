using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Filter;
using Smart.Core.JWT;

namespace Lottery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetToken")]
        [CustomRouteAttribute("GetToken")]
        public async Task<object> GetToken(string name, string password)
        {
            string userid = "10001";
            string role = "Admin";
            var token = new JWTHelper().CreateToken(userid, name, role);
            return Ok(new
            {
                success = true,
                token = token
            });
        }
    }
}
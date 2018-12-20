using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Extensions;
using Smart.Core.Filter;
using Smart.Core.JWT;
using Smart.Core.Throttle;

namespace Lottery.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public IActionResult GetNowDate()
        {
            return Ok(new LotteryServiceResponse()
            {
                Code = ResponseCode.成功,
                Value = DateTimeHelper.GetCurrentUnixTimeStamp()
            });
        }

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        public IActionResult GetArea()
        {
            return null;
        }

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
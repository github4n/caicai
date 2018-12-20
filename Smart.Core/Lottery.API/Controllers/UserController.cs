using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lottery.Api.Controllers.CommonFilterActtribute;
using Lottery.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.JWT;

namespace Lottery.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected IUsersService _usersService;
        public UserController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Test()
        {
            var list = await this._usersService.TestMethod();
            return Ok(new
            {
                success = true,
                data = list
            });
        }


        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [ReusltFilter]
        [HttpPost]
        public async Task<object> Test111()
        {

            return Ok(new
            {
                success = true,
                data = "1111"
            });
        }
    }
}
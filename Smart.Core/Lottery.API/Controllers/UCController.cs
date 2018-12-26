using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Utils;

namespace Lottery.API.Controllers
{
    [Route("uc/[action]")]
    [ApiController]
    public class UCController : ControllerBase
    {
        /// <summary>
        /// 通用彩票列表
        /// http://lottery.jdddata.com/uc/common
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Common()
        {
          
            return await Task.Run(() => "1111");
        }

        /// <summary>
        /// 高频彩：http://lottery.jdddata.com/uc/highfreq
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> highfreq()
        {
            return await Task.Run(() => "1111");
        }

        /// <summary>
        /// 非高频彩彩票结果：http://lottery.jdddata.com/uc/nonhighfreq
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> nonhighfreq()
        {
            return await Task.Run(() => "1111");
        }

        /// <summary>
        /// 竞彩：http://lottery.jdddata.com/uc/jingcai
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> jingcai()
        {
            return await Task.Run(() => "1111");
        }

    }
}
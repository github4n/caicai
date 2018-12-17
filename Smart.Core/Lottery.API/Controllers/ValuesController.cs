using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lottery.Modes.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Filter;
using Smart.Core.Throttle;

namespace Lottery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[XSSFilter]
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        //[JWTFilter(Policy = "Admin")]
        [RateValve(Policy = Policy.Ip, Limit = 1, Duration = 60)]
        public ActionResult<IEnumerable<string>> Get()
        {
            var userInfo = GetHeaderToken;
            return new string[] { "value1", "value2" };
        }

      

        /// <summary>
        /// 获取一个数据列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IEnumerable<string>> Post([FromBody]Users user)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// post
        /// </summary>
        /// <param name="love">model实体类参数</param>
        //[HttpPost]
        //public void Post(Users user)
        //{
        //}

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://lottery.jdddata.com/uc/common");
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(streamReceive, Encoding.UTF8))
                    {
                        xmlDoc.Load(sr);
                    }
                }
                webResponse.Close();
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
            return await Task.Run(() =>"");
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
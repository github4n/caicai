using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using Lottery.Modes.Model;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Smart.Core.NoSql.Redis;
using Smart.Core.Redis;
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
            try
            {
                var xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/common");
                var commonConfig = ConfigFileHelper.Get<List<CommonModel>>("CommonModel");
                var nodes = xmlDoc.DocumentElement.ChildNodes[0].ChildNodes;
                foreach (XmlElement CurrenNode in nodes)
                {
                    var Common = commonConfig.Find((x) => x.key == CurrenNode.ChildNodes[0].InnerText);
                    var item = CurrenNode.ChildNodes[2];
                    foreach (XmlElement Subitem in item.ChildNodes)
                    {
                        if (Subitem.Name == "item")
                        {
                            var Lottery = Subitem.ChildNodes[0].Attributes["col0"].InnerText;
                            foreach (XmlElement Sub_subItem in Subitem)
                            {
                                if (Sub_subItem.Name == "link")
                                {
                                    
                                    if (Sub_subItem.Attributes["linkcontent"].InnerText == "开奖详情")
                                    {
                                        Sub_subItem.SetAttribute("linkurl", Common.Item.Where(x => x.Lottery == Lottery).FirstOrDefault().DetailLinkUrl);
                                    }
                                    else if(Sub_subItem.Attributes["linkcontent"].InnerText == "玩法说明")
                                    {
                                        Sub_subItem.SetAttribute("linkurl", Common.Item.Where(x => x.Lottery == Lottery).FirstOrDefault().RemarkLinkUrl);
                                    }
                                }
                            }
                        }
                    }
                }
                RedisManager.DB_Other.Set("UC_Common", xmlDoc.InnerXml);
                var b = RedisManager.DB_Other.Get("UC_Common");
                return await Task.Run(() => b);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 高频彩：http://lottery.jdddata.com/uc/highfreq
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> highfreq()
        {
            var xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/highfreq");
            var commonConfig = ConfigFileHelper.Get<List<CommonModel>>("HeightLottery");
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
        protected XmlDocument LoadXmlDocument(string URI)
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
            return xmlDoc;
        }
    }
}
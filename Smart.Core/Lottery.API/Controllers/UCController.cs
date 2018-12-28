using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Lottery.API.Result;
using Lottery.Modes.Model;

using Microsoft.AspNetCore.Mvc;

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
        public XmlResult Common()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                var b = RedisManager.DB_Other.Get<byte[]>("UC_Common");
                if (b == null || b.Length == 0)
                {
                     xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/common");
                    var commonConfig = ConfigFileHelper.Get<List<CommonModel>>("CommonModel");
                    var nodes = xmlDoc.DocumentElement.ChildNodes[0].ChildNodes;
                    foreach (XmlElement CurrenNode in nodes)
                    {
                        var Common = commonConfig.Find((x) => x.key == CurrenNode.ChildNodes[0].InnerText);
                        if (Common == null)
                        {
                            continue;
                        }
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
                                        else if (Sub_subItem.Attributes["linkcontent"].InnerText == "玩法说明")
                                        {
                                            Sub_subItem.SetAttribute("linkurl", Common.Item.Where(x => x.Lottery == Lottery).FirstOrDefault().RemarkLinkUrl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    MemoryStream ms = new MemoryStream();
                    serializer.Serialize(ms, xmlDoc);
                    RedisManager.DB_Other.Set("UC_Common", ms.ToArray(), 60 * 3);
                }
                else
                {
                    MemoryStream ms1 = new MemoryStream(b);
                    xmlDoc = serializer.Deserialize(ms1) as XmlDocument;
                }
                return new XmlResult(xmlDoc);
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
        public XmlResult highfreq()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                var b = RedisManager.DB_Other.Get<byte[]>("UC_highfreq");
                if (b == null || b.Length == 0)
                {
                    xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/highfreq");
                    var commonConfig = ConfigFileHelper.Get<List<HeightLottery>>("HeightLottery");
                    var nodes = xmlDoc.DocumentElement.ChildNodes;
                    foreach (XmlElement element in nodes)
                    {
                        var Common = commonConfig.Find((x) => x.key == element.ChildNodes[0].InnerText);
                        if (Common == null)
                        {
                            continue;
                        }
                        foreach (XmlElement Sub_element in element.LastChild)
                        {
                            if (Sub_element.Name == "title_url")
                            {
                                Sub_element.InnerText = Common.title_url;
                            }
                            if (Sub_element.Name == "bet")
                            {
                                var remark = Common.foot_group.Where(x => x.remark == Sub_element.FirstChild.InnerText).FirstOrDefault();
                                if (remark == null)
                                {
                                    continue;
                                }
                                  ((XmlElement)Sub_element.LastChild).InnerText = remark.url;
                            }
                            if (Sub_element.Name == "foot_group")
                           {
                                foreach (XmlElement Sub_foot_group in Sub_element.ChildNodes)
                                {
                                    if (Sub_foot_group.Attributes["name"].InnerText == "开奖详情")
                                    {
                                        Sub_foot_group.SetAttribute("url", Common.foot_group.Where(x => x.remark == Sub_foot_group.Attributes["name"].InnerText).FirstOrDefault().url);
                                    }
                                    else if (Sub_foot_group.Attributes["name"].InnerText == "玩法说明")
                                    {
                                        Sub_foot_group.SetAttribute("url", Common.foot_group.Where(x => x.remark == Sub_foot_group.Attributes["name"].InnerText).FirstOrDefault().url);
                                    }
                                }
                            }
                        }
                    }
                    MemoryStream ms = new MemoryStream();
                    serializer.Serialize(ms, xmlDoc);
                    RedisManager.DB_Other.Set("UC_highfreq", ms.ToArray(), 60 * 3);
                }
                else
                {
                    MemoryStream ms1 = new MemoryStream(b);
                    xmlDoc = serializer.Deserialize(ms1) as XmlDocument;
                }
                return new XmlResult(xmlDoc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 非高频彩彩票结果：http://lottery.jdddata.com/uc/nonhighfreq
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public XmlResult nonhighfreq()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                var b = RedisManager.DB_Other.Get<byte[]>("UC_nonhighfreq");
                if (b == null || b.Length == 0)
                {
                     xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/nonhighfreq");
                    var commonConfig = ConfigFileHelper.Get<List<NoHeightLottery>>("NoHeightLottery");
                    var nodes = xmlDoc.DocumentElement.ChildNodes;
                    foreach (XmlElement element in nodes)
                    {
                        var Common = commonConfig.Where(x => x.key == element.FirstChild.InnerText).FirstOrDefault();
                        if (Common == null)
                        {
                            continue;
                        }
                        var Issue = ((XmlElement)element.GetElementsByTagName("qihao")[0]).InnerText;
                        if (string.IsNullOrEmpty(Issue))
                        {
                            continue;
                        }
                        var titelUrl = Common.title_url + Issue;
                        var morelink = Common.morelink + Issue;
                        foreach (XmlElement Sub_element in element.LastChild)
                        {
                            if (Sub_element.Name == "title_url")
                            {
                                Sub_element.InnerText = titelUrl;
                            }
                            if (Sub_element.Name == "bet")
                            {
                                var remark = Common.foot_group.Where(x => x.remark == Sub_element.FirstChild.InnerText).FirstOrDefault();
                                if (remark == null)
                                {
                                    continue;
                                }
                                ((XmlElement)Sub_element.LastChild).InnerText = remark.url;
                            }
                            if (Sub_element.Name == "foot_group")
                            {
                                foreach (XmlElement Sub_foot_group in Sub_element.ChildNodes)
                                {
                                    if (Sub_foot_group.Attributes["name"].InnerText == "开奖详情")
                                    {
                                        Sub_foot_group.SetAttribute("url", titelUrl);
                                    }
                                    else if (Sub_foot_group.Attributes["name"].InnerText == "玩法说明")
                                    {
                                        Sub_foot_group.SetAttribute("url", Common.foot_group.Where(x => x.remark == Sub_foot_group.Attributes["name"].InnerText).FirstOrDefault().url);
                                    }
                                }
                            }
                            if (Sub_element.Name == "morelink")
                            {
                                Sub_element.SetAttribute("url", morelink);
                            }
                        }
                    }
                    MemoryStream ms = new MemoryStream();
                    serializer.Serialize(ms, xmlDoc);
                    RedisManager.DB_Other.Set("UC_nonhighfreq", ms.ToArray(), 60 * 3);
                }
                else
                {
                    MemoryStream ms1 = new MemoryStream(b);
                    xmlDoc = serializer.Deserialize(ms1) as XmlDocument;
                }
                return new XmlResult(xmlDoc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 竞彩：http://lottery.jdddata.com/uc/jingcai
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public XmlResult jingcai()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                var b = RedisManager.DB_Other.Get<byte[]>("UC_jingcai");
                if (b == null || b.Length == 0)
                {
                    xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/jingcai");
                    var commonConfig = ConfigFileHelper.Get<List<JingCai>>("JingCai");
                    var nodes = xmlDoc.DocumentElement.ChildNodes;
                    foreach (XmlElement element in nodes)
                    {
                        var Common = commonConfig.Where(x => x.key == element.FirstChild.InnerText).FirstOrDefault();
                        if (Common == null)
                        {
                            continue;
                        }
                        foreach (XmlElement Sub_element in element.LastChild)
                        {
                            if (Sub_element.Name == "url")
                            {
                                Sub_element.InnerText = Common.url;
                            }
                            else if (Sub_element.Name == "links")
                            {
                                foreach (XmlElement Sub_item in Sub_element.ChildNodes)
                                {
                                    if (Sub_item.FirstChild.InnerText == "更多赛果")
                                    {
                                        Sub_item.LastChild.InnerText = Common.links.Where(x => x.key == Sub_item.FirstChild.InnerText).FirstOrDefault().title_url;
                                    }
                                    else 
                                    {
                                        Sub_item.LastChild.InnerText = Common.links.Where(x => x.key == Sub_item.FirstChild.InnerText).FirstOrDefault().title_url;
                                    }
                                }
                            }
                        }
                    }
                    //MemoryStream ms = new MemoryStream();
                    //serializer.Serialize(ms, xmlDoc);
                    //RedisManager.DB_Other.Set("UC_jingcai", ms.ToArray(), 60 * 3);
                }
                else
                {
                    MemoryStream ms1 = new MemoryStream(b);
                    xmlDoc = serializer.Deserialize(ms1) as XmlDocument;
                }
                return new XmlResult(xmlDoc);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected XmlDocument LoadXmlDocument(string URI)
        {
            XmlDocument xmlDoc = new XmlDocument();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URI);
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
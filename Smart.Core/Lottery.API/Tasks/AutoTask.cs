using Lottery.Modes.Model;
using Lottery.Services;
using Lottery.Services.Abstractions;
using Smart.Core.NoSql.Redis;
using Smart.Core.Repository.SqlSugar;
using Smart.Core.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Lottery.API.Tasks
{
    /// <summary>
    /// 计时任务
    /// </summary>
    public class AutoTask
    {
        public IApi_DataService IApi_Service = null;
        private static readonly string Source = ConfigFileHelper.Get("Source");
        public AutoTask()
        {
            ConnectionConfig conn = new ConnectionConfig()
            {
                ConnectionString = ConfigFileHelper.Get("Lottery:Data:Database:Connection"),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                IsShardSameThread = false
            };
            var fa = new DbFactory(conn);
            IApi_Service = new Api_DataService(fa);
        }
        public void AutoAddToRedis_LotteryList()
        {
            while (true)
            {
                try
                {
                    IApi_Service.AddRedisLocalLottery();
                }
                catch (Exception)
                {

                }
                try
                {
                    IApi_Service.AddRedisHighLottery();
                }
                catch (Exception)
                {

                }
                try
                {
                    IApi_Service.AddRedisCountryLottery();
                }
                catch (Exception)
                {

                }
                Thread.Sleep(3 * 1000 * 60);
            }
        }


        public void UCCommon_Redis()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/common");
                if (xmlDoc == null || xmlDoc.DocumentElement.ChildNodes.Count == 0)
                {
                    return;
                }
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
                RedisManager.DB_Other.Set("UC_Common", ms.ToArray());
            }
            catch (Exception ex)
            {

            }
        }

        public void UCHighfreq_Redis()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/highfreq");
                if (xmlDoc == null || xmlDoc.DocumentElement.ChildNodes.Count == 0)
                {
                    return;
                }
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
                        if (Sub_element.Name == "source")
                        {
                            Sub_element.InnerText = Source;
                        }
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
                RedisManager.DB_Other.Set("UC_highfreq", ms.ToArray());
            }
            catch (Exception ex)
            {

            }
        }


        public void UCNohighfreq_Redis()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/nonhighfreq");
                var commonConfig = ConfigFileHelper.Get<List<NoHeightLottery>>("NoHeightLottery");
                if (xmlDoc == null || xmlDoc.DocumentElement.ChildNodes.Count == 0)
                {
                    return;
                }
                var nodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlElement element in nodes)
                {
                    var Common = commonConfig.Where(x => x.key == element.FirstChild.InnerText).FirstOrDefault();
                    if (Common == null)
                    {
                        continue;
                    }
                    var titelUrl = Common.title_url;
                    var morelink = Common.morelink;
                    foreach (XmlElement Sub_element in element.LastChild)
                    {
                        if (Sub_element.Name == "source")
                        {
                            Sub_element.InnerText = Source;
                        }
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
                RedisManager.DB_Other.Set("UC_nonhighfreq", ms.ToArray());
            }
            catch (Exception ex)
            {
            }
        }



        public void UCJingcai_Redis()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                xmlDoc = LoadXmlDocument("http://lottery.jdddata.com/uc/jingcai");
                var commonConfig = ConfigFileHelper.Get<List<JingCai>>("JingCai");
                if (xmlDoc == null || xmlDoc.DocumentElement.ChildNodes.Count == 0)
                {
                    return;
                }
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
                        if (Sub_element.Name == "source")
                        {
                            Sub_element.InnerText = Source;
                        }
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
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, xmlDoc);
                RedisManager.DB_Other.Set("UC_jingcai", ms.ToArray());
            }
            catch (Exception ex)
            {
                
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


        public void UcAutoRedis()
        {
            while (true)
            {
                UCCommon_Redis();
                UCHighfreq_Redis();
                UCNohighfreq_Redis();
                UCJingcai_Redis();
                Thread.Sleep(5 * 1000);
            }
        }
    }
}

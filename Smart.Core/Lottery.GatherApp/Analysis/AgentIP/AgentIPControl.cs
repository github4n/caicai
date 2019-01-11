using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Lottery.Services.Abstractions;
using log4net;
using Lottery.Modes.Model;

namespace Lottery.GatherApp
{
   public class AgentIPControl
    {
        private readonly static string DataURI = "https://www.xicidaili.com/nn/{0}";
        private int PageIndex = 4;
        protected IAgentIPService _agentIPService;
        private ILog log;
        public AgentIPControl(IAgentIPService agentIPService)
        {
            _agentIPService = agentIPService;
            log = LogManager.GetLogger("LotteryRepository", typeof(AgentIPControl));
        }
        public void StartLoadAgentIP()
        {
            GetIP();
        }
        private void GetIP()
        {
            int TotalCount = 0;
            for (int p = 1; p <= PageIndex; p++)
            {
                var Doc = RequestHtmlDoc(p);
                var tableNode = Doc.DocumentNode.SelectSingleNode("//table[@id='ip_list']");
                var trNode = tableNode.SelectNodes("tr");
                List<IP> IPList = new List<IP>();
                for (int i = 0; i < trNode.Count; i++)
                {
                    if (i == 0) continue;
                    IP _ip = new IP
                    {
                        IPAddress = trNode[i].ChildNodes[3].InnerText,
                        Port = trNode[i].ChildNodes[5].InnerText,
                        Type = trNode[i].ChildNodes[11].InnerText,
                        Speed = trNode[i].ChildNodes[13].ChildNodes[1].Attributes["title"].Value.Replace("秒", ""),
                        ConnectionTime = trNode[i].ChildNodes[15].ChildNodes[1].Attributes["title"].Value.Replace("秒", ""),
                        CreateTime = DateTime.Now
                    };
                    IPList.Add(_ip);
                }
                _agentIPService.AddAgentIPList(IPList, out int Count);
                TotalCount = TotalCount + Count;
            }
            Console.WriteLine($"成功新增代理IP共{TotalCount}个");
        }
        private HtmlDocument RequestHtmlDoc(int p)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            if (string.IsNullOrEmpty(DataURI)) throw new Exception("AgentIP URI IS Null");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(DataURI,p));
            WebProxy proxy = new WebProxy("119.101.115.59",9999);
            request.Proxy = proxy;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream ResponseStream = response.GetResponseStream())
            {
                using (StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.UTF8))
                {
                    htmlDoc.LoadHtml(StreamReader.ReadToEnd());
                }
            }
            return htmlDoc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Lottery.Services.Abstractions;
using log4net;
using Lottery.Modes.Model;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Lottery.GatherApp
{
   public class AgentIPControl
    {
        private readonly static string DataURI = "https://www.xicidaili.com/nn/{0}";
        private int PageIndex = 4;
        protected IAgentIPService _agentIPService;
        private ILog log;
        private List<IP> TotaliPs = new List<IP>();
        private WebProxy CurrentWebProxy;
        private IP CurrentIP;
        private int UseAgent;
        public AgentIPControl(IAgentIPService agentIPService)
        {
            _agentIPService = agentIPService;
            log = LogManager.GetLogger("LotteryRepository", typeof(AgentIPControl));
            int.TryParse(Smart.Core.Utils.ConfigFileHelper.Get("UseIPAgent"),out int UseAgent);
        }
        public void StartLoadAgentIP()
        {
            SetProxy();
            GetIP();
        }
        private void GetIP()
        {
            int TotalCount = 0;
            for (int p = 1; p <= PageIndex; p++)
            {
                var Doc = RequestHtmlDoc(string.Format(DataURI,p));
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
                        Speed = Convert.ToSingle(trNode[i].ChildNodes[13].ChildNodes[1].Attributes["title"].Value.Replace("秒", "")),
                        ConnectionTime = Convert.ToSingle(trNode[i].ChildNodes[15].ChildNodes[1].Attributes["title"].Value.Replace("秒", "")),
                        CreateTime = DateTime.Now,
                        FailNum = 0,
                        IsDelete = false
                    };
                    IPList.Add(_ip);
                }
                _agentIPService.AddAgentIPList(IPList, out int Count);
                TotalCount = TotalCount + Count;
            }
            Console.WriteLine($"成功新增代理IP共{TotalCount}个");
        }
        private HtmlDocument RequestHtmlDoc(string URl)
        {
            var num = 0;
            while (true)
            {
                try
                {
                    RequestType Type;
                    if (CurrentIP!=null&&"HTTPS" == CurrentIP.Type)
                    {
                        Type = RequestType.HTTPS;
                    }
                    else {Type= RequestType.HTTP; }
                    string UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 71.0.3578.98 Safari / 537.36";
                    if (UseAgent==1&&CurrentIP == null)
                    {
                        return RequestHelper<HtmlDocument>.DoRequest(URl, null, false,"","", UserAgent, string.Empty, Method.Get,null);
                    }
                    else
                    {
                        return RequestHelper<HtmlDocument>.DoRequest(URl, null, true, CurrentIP.IPAddress, CurrentIP.Port, UserAgent, string.Empty, Method.Get, null);
                    }
                }
                catch (Exception ex)
                {
                    if (CurrentIP != null)
                    {
                        if (num <= 50)
                        {
                            _agentIPService.DeleteNotUseAgentIP(CurrentIP.ID);
                            SetProxy();
                        }
                        else if (num == 51)
                        {
                            CurrentWebProxy = null;
                            CurrentIP = null;
                        }
                        else
                        {
                            throw new Exception("连接失败");
                        }
                        num++;
                    }
                }
            }
        }
        private void SetProxy()
        {
            TotaliPs = _agentIPService.GetIPs();
            if (TotaliPs == null || TotaliPs.Count == 0) return;
            int rand = new Random().Next(0, TotaliPs.Count);
            CurrentWebProxy = new WebProxy(TotaliPs[rand].IPAddress, Convert.ToInt32(TotaliPs[rand].Port));
            CurrentIP = TotaliPs[rand];
        }
    }
}

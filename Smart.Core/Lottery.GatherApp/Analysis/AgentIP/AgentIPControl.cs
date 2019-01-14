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
                        Port =trNode[i].ChildNodes[5].InnerText,
                        Type = trNode[i].ChildNodes[11].InnerText,
                        Speed =Convert.ToSingle(trNode[i].ChildNodes[13].ChildNodes[1].Attributes["title"].Value.Replace("秒", "")),
                        ConnectionTime = Convert.ToSingle(trNode[i].ChildNodes[15].ChildNodes[1].Attributes["title"].Value.Replace("秒", "")),
                        CreateTime = DateTime.Now,
                        FailNum=0
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
            TotaliPs = _agentIPService.GetIPs();
            SetProxy();
            var num = 0;
            while (true)
            {
                try
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    if (string.IsNullOrEmpty(DataURI)) throw new Exception("URL为空");
                    HttpWebRequest request;
                    if (CurrentIP != null && CurrentIP.Type == "HTTPS")
                    {
                        request = (HttpWebRequest)WebRequest.Create(URl);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = (HttpWebRequest)WebRequest.Create(URl);
                    }
                    request.Timeout = 10000;
                    if (CurrentWebProxy != null && CurrentIP != null)
                    {
                        request.Proxy = CurrentWebProxy;
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //string res = string.Empty;
                    //if (response.ContentEncoding != null && response.ContentEncoding.ToLower() == "gzip")
                    //{
                    //    System.IO.Stream streamReceive = response.GetResponseStream();
                    //    var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress);
                    //    StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding("GB2312"));
                    //    res = sr.ReadToEnd();
                    //}
                    //else
                    //{
                    //    System.IO.Stream streamReceive = response.GetResponseStream();
                    //    StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding("GB2312"));
                    //    res = sr.ReadToEnd();
                    //}
                    using (Stream ResponseStream = response.GetResponseStream())
                    {
                        using (StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.UTF8))
                        {
                            htmlDoc.LoadHtml(StreamReader.ReadToEnd());
                        }
                    }
                    return htmlDoc;
                }
                catch (Exception ex)
                {
                    if (CurrentIP != null)
                    {
                        if (num <= 50)
                        {
                            TotaliPs.Remove(CurrentIP);
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
        private static bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        private void SetProxy()
        {
            if (TotaliPs == null || TotaliPs.Count == 0) return;
            int rand = new Random().Next(0, TotaliPs.Count);
            CurrentWebProxy = new WebProxy(TotaliPs[rand].IPAddress, Convert.ToInt32(TotaliPs[rand].Port));
            //CurrentWebProxy = new WebProxy("119.101.118.172", 9999);
            CurrentIP = TotaliPs[rand];
        }
    }
}

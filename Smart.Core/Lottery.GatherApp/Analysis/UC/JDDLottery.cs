using Lottery.GatherApp.Helper;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lottery.GatherApp.Analysis.UC
{
    public class JDDLottery
    {

        protected IJddDataService _IJddDataService;
        private string Url_JDDKJ;
        public JDDLottery(IJddDataService IJddDataService)
        {
            _IJddDataService = IJddDataService;
            if (string.IsNullOrEmpty(Url_JDDKJ))
            {
                Url_JDDKJ = Smart.Core.Utils.ConfigFileHelper.Get("Url_JDDKJ");
            }

        }
        public async Task<int> LoadJdd(string gameCode)
        {
            int count = 0;
            string htmlCode;
            List<sys_issue> IssueList = new List<sys_issue>();

            HttpWebRequest request;
            HttpWebResponse response = null;
            string Url = "uc/"+ gameCode;
            request = (HttpWebRequest)WebRequest.Create(Url_JDDKJ+Url);
            response = CommonHelper.SettingProxyCookit(request, response);

            if (response.ContentEncoding != null && response.ContentEncoding.ToLower() == "gzip")
            {

                System.IO.Stream streamReceive = response.GetResponseStream();
                var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress);
                StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.UTF8);
                htmlCode = sr.ReadToEnd();

            }
            else
            {
                System.IO.Stream streamReceive = response.GetResponseStream();

                StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.UTF8);

                htmlCode = sr.ReadToEnd();
            }

            XmlDocument doc = new XmlDocument();//新建对象
            doc.LoadXml(htmlCode);
            //奖多多非高频
            if (gameCode == "nonhighfreq")
            {
                JDDnonhighfreq(doc, IssueList);
            }


            count =await _IJddDataService.AddIssue(IssueList);
            return await Task.FromResult(count);

        }


        public void JDDnonhighfreq(XmlDocument doc, List<sys_issue> IssueList)
        {
            var list = doc.DocumentElement.ChildNodes;  
            foreach (XmlElement element in list)
            {
           
                sys_issue issue = new sys_issue();
                issue.LotteryCode = ((XmlElement)element.GetElementsByTagName("key")[0]).InnerText;
                issue.IssueNo = ((XmlElement)element.GetElementsByTagName("qihao")[0]).InnerText;
                issue.OpenCode = ((XmlElement)element.GetElementsByTagName("number")[0]).InnerText.Replace("-", "|");
                issue.OpenTime = ((XmlElement)element.GetElementsByTagName("time")[0]).InnerText;
                IssueList.Add(issue);
            }
        }


    }
}

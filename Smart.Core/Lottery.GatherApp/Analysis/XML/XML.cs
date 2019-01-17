
using HtmlAgilityPack;
using Lottery.GatherApp.Helper;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Smart.Core.Utils.CommonHelper;

namespace Lottery.GatherApp
{
    public class XML
    {
      
       
        protected IXML_DataService _IXML_DataService;
        private string url_500KJ;
        public XML(IXML_DataService XML_DataService)
        {
            _IXML_DataService = XML_DataService;
            if (string.IsNullOrEmpty(url_500KJ))
            {
                url_500KJ = Smart.Core.Utils.ConfigFileHelper.Get("Url_500KJ");
            }
        }
        public async Task<int> LoadXml(string gameCode)
        {
            string htmlCode;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DateTime OldDate;
            var strDate = _IXML_DataService.GetNowIssuNo(gameCode);
            if (strDate == null)
            {
                OldDate = DateTime.Now.AddMonths(-1);
            }
            else {

                OldDate = Convert.ToDateTime(strDate.OpenTime);
            }
            DateTime NowDate = DateTime.Now;
            TimeSpan ts = NowDate - OldDate;
            XmlNodeList list = null;
            HttpWebRequest request;
            HttpWebResponse response=null;
            int count = 0; int InsertCount = 0;
            for (int i = 0; i < ts.Days + 1; i++)
            {
                try
                {
                    string Url;
                    if (gameCode == "ssl" || gameCode == "ssc")
                    {
                        
                        Url = "http://kaijiang.500.com/static/public/" + gameCode + "/xml/qihaoxml/" + OldDate.AddDays(i).ToString("yyyyMMdd") + ".xml";
                    }
                    else if (gameCode== "df6j1")
                    {
                        Url = "http://kaijiang.500.com/static/info/kaijiang/xml/" + gameCode + "/list.xml";
                    }
                    else
                    {
                        Url = "http://kaijiang.500.com/static/info/kaijiang/xml/" + gameCode + "/" + OldDate.AddDays(i).ToString("yyyyMMdd") + ".xml";
                    }
                    request = (HttpWebRequest)WebRequest.Create(Url);
                   
                    response = CommonHelper.SettingProxyCookit(request, response, CollectionUrlEnum.url_500kaijiang);
                   
                }
                catch (Exception  ex)
                {
                 
                    continue;
                  
                }
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

                XmlDocument doc = new System.Xml.XmlDocument();//新建对象
                doc.LoadXml(htmlCode);
                //List<DataModel> lists = new List<DataModel>();

                list = doc.SelectNodes("//row");

                count = await _IXML_DataService.AddXMLAsync(list, gameCode, OldDate.AddDays(i).ToString("yyyy-MM-dd"));
                InsertCount += count;
                Thread.Sleep(new Random().Next(10000,15000));
            }


            return await Task.FromResult(InsertCount);
        }

        public async Task<int> LoadQGDFCXml(string gameCode)
        {
            string htmlCode;
            XmlNodeList list = null;
            HttpWebRequest request;
            HttpWebResponse response = null;
            int count = 0; 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string Url = "http://kaijiang.500.com/static/info/kaijiang/xml/"+ gameCode + "/list.xml";
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);

                response = CommonHelper.SettingProxyCookit(request, response, CollectionUrlEnum.url_500kaijiang);
            }
            catch (Exception ex)
            {
             
                return 0;
            }

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
            XmlDocument doc = new System.Xml.XmlDocument();//新建对象
            doc.LoadXml(htmlCode);
            //List<DataModel> lists = new List<DataModel>();

            list = doc.SelectNodes("//row");
          
            count = await _IXML_DataService.AddQGDFCXMLAsync(list, gameCode);
            Thread.Sleep(new Random().Next(10000, 15000));
            return await Task.FromResult(count);
        }

        public async Task<int> LoadSDhtml(string gameCode)
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/sd.shtml");
            List<sys_issue> sys_issue = new List<sys_issue>();
            
            int index = 0;
            foreach (HtmlNode item in anode)
            {
                index++;
                if (index < 31)
                {
                    if (_IXML_DataService.GetDescIssuNo(gameCode) != null)
                    {
                        if (item.InnerHtml == _IXML_DataService.GetDescIssuNo(gameCode).IssueNo)
                        {
                            break;
                        }
                    }
                    sys_issue issue = new sys_issue();
                    issue.IssueNo = item.InnerHtml;
                    var htmlDoc = CommonHelper.LoadGziphtml("http://kaijiang.500.com/shtml/sd/" + item.InnerHtml + ".shtml", CollectionUrlEnum.url_500kaijiang);

                    var FirstTableTrNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr");
                    int k = 1;
                    foreach (var item2 in FirstTableTrNode)//遍历第一个table下的tr
                    {
                        switch (k)
                        {
                            case 1:
                                var Date = item2.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                                string openTime = Date.Split('：')[1].Split('兑')[0];
                                issue.OpenTime = Convert.ToDateTime(openTime).ToString("yyyy-MM-dd"); ;
                                sys_issue.Add(issue);
                                break;
                            case 2:
                                int j = 1;
                                var tdindex = item2.SelectSingleNode("td").SelectSingleNode("table").SelectSingleNode("tr").SelectNodes("td");
                                foreach (var item3 in tdindex)
                                {
                                    switch (j)
                                    {
                                        case 1:
                                            var lilist = tdindex[1].SelectSingleNode("div").SelectSingleNode("ul").SelectNodes("li");
                                            foreach (var item4 in lilist)
                                            {
                                                issue.OpenCode += item4.InnerHtml + ",";

                                            }
                                            issue.OpenCode = issue.OpenCode.Trim(',');
                                            break;
                                        case 2:
                                            issue.OpenCode += "|";
                                            StringBuilder sb = new StringBuilder();
                                            string str= tdindex[2].SelectSingleNode("div").InnerHtml.Split('：')[1];
                                            for (int i = 0; i < str.ToString().Replace(" ","").Length; i++)
                                            {
                                                sb.Append(str.ToString().Replace(" ", "")[i]).Append(",");
                                            }
                                            String value = sb.ToString().Trim(',');
                                            issue.OpenCode += value;
                                            break;

                                    }
                                    j++;
                                }

                                break;

                        }
                        k = k + 1;

                    }
                }
                Console.WriteLine(index);
            }
          
            int count = await _IXML_DataService.AddSDhtml(sys_issue, gameCode);

            return count;
        }

        public async Task<int> LoadPlsHtml(string gameCode)
        {
            var anode = CommonHelper.GetExpect("http://kaijiang.500.com/pls.shtml");
            List<sys_issue> sys_issue = new List<sys_issue>();
            int index = 0;
            foreach (HtmlNode item in anode)
            {
                index++;
                if (index < 31)
                {
                    if (_IXML_DataService.GetDescIssuNo(gameCode) != null)
                    {
                        if (item.InnerHtml == _IXML_DataService.GetDescIssuNo(gameCode).IssueNo)
                        {
                            break;
                        }
                    }
                    sys_issue issue = new sys_issue();
                    issue.IssueNo = item.InnerHtml;
                    var htmlDoc = CommonHelper.LoadGziphtml("http://kaijiang.500.com/shtml/pls/" + item.InnerHtml + ".shtml", CollectionUrlEnum.url_500kaijiang);

                    var FirstTableTrNode = htmlDoc.DocumentNode.SelectNodes("//table[@class='kj_tablelist02']")[0].SelectNodes("tr");
                    int k = 1;
                    foreach (var item2 in FirstTableTrNode)//遍历第一个table下的tr
                    {
                        switch (k)
                        {
                            case 1:
                                var Date = item2.SelectSingleNode("//span[@class='span_right']").InnerHtml;
                                string openTime = Date.Split('：')[1].Split('兑')[0];
                                issue.OpenTime = Convert.ToDateTime(openTime).ToString("yyyy-MM-dd");
                                sys_issue.Add(issue);
                                break;
                            case 2:
                                var tdindex = item2.SelectSingleNode("td").SelectSingleNode("table").SelectSingleNode("tr").SelectNodes("td");
                                foreach (var item3 in tdindex)
                                {
                                            var lilist = tdindex[1].SelectSingleNode("div").SelectSingleNode("ul").SelectNodes("li");
                                            foreach (var item4 in lilist)
                                            {
                                                issue.OpenCode += item4.InnerHtml + ",";
                                            }
                                            issue.OpenCode = issue.OpenCode.Trim(',');
                                            break;
                                }
                                break;

                        }
                        k = k + 1;

                    }
                }
             
            }

            int count = await _IXML_DataService.AddSDhtml(sys_issue, gameCode);
            return count;

        }

        /// <summary>
        /// 北京单场期号
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetBjdcAsync()
        {
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php");
            int count= await _IXML_DataService.AddBjdcIssue(anode, "zqdc");
            return count;
        }

        /// <summary>
        /// 胜负过关期号
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetSfggAsync()
        {
            var anode = CommonHelper.GetBJDCExpect("http://zx.500.com/zqdc/kaijiang.php?playid=6");
            int count = await _IXML_DataService.AddBjdcIssue(anode, "zqdcsfgg");
            return count;
        }



    }
}

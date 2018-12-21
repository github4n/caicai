using EntityModel.Common;
using HtmlAgilityPack;
using Lottery.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Lottery.GatherApp
{
    public class XML
    {
        protected IXML_DataService _IXML_DataService;

        public XML(IXML_DataService XML_DataService)
        {
            _IXML_DataService = XML_DataService;
        }
        public CookieContainer CookiesContainer { get; set; }//定义Cookie容器
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
            HttpWebResponse response;
            int count = 0; int InsertCount = 0;
            for (int i = 0; i < ts.Days + 1; i++)
            {
                try
                {
                    string Url;
                    if (gameCode == "ssl")
                    {
                        Url = "http://kaijiang.500.com/static/public/ssl/xml/qihaoxml/" + OldDate.AddDays(i).ToString("yyyyMMdd") + ".xml";
                    }
                    else
                    {
                        Url = "http://kaijiang.500.com/static/info/kaijiang/xml/" + gameCode + "/" + OldDate.AddDays(i).ToString("yyyyMMdd") + ".xml";
                    }
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    var userAgentModel = CommonHelper.GetUserAgent();
                    response=CommonHelper.SettingProxyCookit(request,0);
                    //response = (HttpWebResponse)request.GetResponse();
                    //foreach (Cookie cookie in response.Cookies) userAgentModel.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                }
                catch (Exception  ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                  
                }
                if (response.ContentEncoding != null && response.ContentEncoding.ToLower() == "gzip")
                {

                    System.IO.Stream streamReceive = response.GetResponseStream();
                    var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress);
                    StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding("GB2312"));
                    htmlCode = sr.ReadToEnd();

                }
                else
                {
                    System.IO.Stream streamReceive = response.GetResponseStream();

                    StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding("GB2312"));

                    htmlCode = sr.ReadToEnd();
                }

                XmlDocument doc = new System.Xml.XmlDocument();//新建对象
                doc.LoadXml(htmlCode);
                //List<DataModel> lists = new List<DataModel>();

                list = doc.SelectNodes("//row");

                count = await _IXML_DataService.AddXMLAsync(list, gameCode, OldDate.AddDays(i).ToString("yyyy-MM-dd"));
                InsertCount += count;
                Thread.Sleep(new Random().Next(1000,5000));
            }


            return await Task.FromResult(InsertCount);
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

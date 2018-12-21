using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EntityModel.Common
{
    public class CommonHelper
    {
        public static Dictionary<string, string> StNode(HtmlNode htmlNode, string key, string value, int spanIndex)
        {
            var keyValue = new Dictionary<string, string>();
            spanIndex = 1;

            foreach (var item3 in htmlNode.SelectNodes("span"))
            {
                switch (spanIndex)
                {
                    case 1:
                        key = item3.InnerHtml;

                        break;
                    case 2:
                        value = item3.InnerHtml;
                        break;

                }
                spanIndex++;
            }
            keyValue.Add(key, value);
            return keyValue;
        }

        public static HtmlDocument Loadhtml(string strhtml)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //注册EncodingProvider的方法，获得网页编码GB2312的支持
            var html = strhtml;
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.GetEncoding("gb2312");
            HtmlDocument htmlDoc = web.Load(html);
            return htmlDoc;
        }
        public static HtmlDocument LoadGziphtml(string strhtml)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //注册EncodingProvider的方法，获得网页编码GB2312的支持
            var html = strhtml;
            HtmlWeb web = new HtmlWeb();
            Gzip(web);  //解压html
            HtmlDocument htmlDoc = web.Load(html);
            return htmlDoc;
        }
        public static CookieContainer CookiesContainer { get; set; }//定义Cookie容器

        public static List<UserAgent_Cookies> UserAgentList { get; set; }

        public static UserAgent_Cookies GetUserAgent()
        {
            if (UserAgentList == null || UserAgentList.Count == 0)
            {
                UserAgentList = new List<UserAgent_Cookies>();
                UserAgentList.Add(new UserAgent_Cookies()
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
                    CookiesContainer = new CookieContainer()
                });
                UserAgentList.Add(new UserAgent_Cookies()
                {
                    UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; InfoPath.3; rv:11.0) like Gecko",
                    CookiesContainer = new CookieContainer()
                });
                UserAgentList.Add(new UserAgent_Cookies()
                {
                    UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 10.0; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0",
                    CookiesContainer = new CookieContainer()
                });
            }
            var rd = new Random();
            var rdNum = rd.Next(0, 2);
            return UserAgentList[rdNum];
        }
        public static void Gzip(HtmlWeb web)
        {
           
            HtmlWeb.PreRequestHandler handler = delegate (HttpWebRequest request)
            {
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                //request.CookieContainer = new System.Net.CookieContainer();
                SettingProxyCookit(request,1);
                return true;
            };
            web.PreRequest += handler;
            web.OverrideEncoding = Encoding.GetEncoding("gb2312");
        }

        public static HttpWebResponse SettingProxyCookit(HttpWebRequest request,int index) {

            var userAgentModel = GetUserAgent();
            request.Method = "GET";
            request.Accept = "*/*";
            request.ServicePoint.Expect100Continue = false;//加快载入速度
            request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
            request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                                                      //request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
            request.AllowAutoRedirect = false;//禁止自动跳转
                                              //设置User-Agent，伪装成Google Chrome浏览器
            request.UserAgent = userAgentModel.UserAgent;
            request.Timeout = 5000;//定义请求超时时间为5秒
            request.KeepAlive = true;//启用长连接
            request.Method = "GET";//定义请求方式为GET              
            request.ContentType = "text/xml";
            request.CookieContainer = userAgentModel.CookiesContainer;//附加Cookie容器
            request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
            //if (index == 1)
            //{
                var response = (HttpWebResponse)request.GetResponse();
                foreach (Cookie cookie in response.Cookies) userAgentModel.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
            //}
            return response;
        }



        public static  HtmlNodeCollection GetExpect(string Url)
        {
            var footnode = LoadGziphtml(Url).DocumentNode.SelectSingleNode("//div[@class='kjxq_box02_title_right']");
            var commentnode = footnode.SelectSingleNode("span[@class='iSelectBox']");
            var lastnode = commentnode.SelectSingleNode("//div[@class='iSelectList']");
            var anode = lastnode.SelectNodes("a"); //获取a标签里面的期号
            return anode;
        }
        public static HtmlNodeCollection GetBJDCExpect(string Url)
        {
            var footnode = LoadGziphtml(Url).DocumentNode.SelectSingleNode("//div[@class='an_box']/div[1]");
            var commentnode = footnode.SelectSingleNode("span");
            var lastnode = commentnode.SelectSingleNode("//select");
            var anode = lastnode.SelectNodes("option"); //获取option标签里面的期号
            return anode;
        }
    }
    public class UserAgent_Cookies
    {
        public string UserAgent { get; set; }

        public CookieContainer CookiesContainer { get; set; }
    }
}

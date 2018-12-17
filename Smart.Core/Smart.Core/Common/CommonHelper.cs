using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Smart.Core
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
        public static void Gzip(HtmlWeb web)
        {
            HtmlWeb.PreRequestHandler handler = delegate (HttpWebRequest request)
            {
                request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.CookieContainer = new System.Net.CookieContainer();
                return true;
            };
            web.PreRequest += handler;
            web.OverrideEncoding = Encoding.GetEncoding("gb2312");
        }



        public static  HtmlNodeCollection GetExpect(string Url)
        {
            var footnode = LoadGziphtml(Url).DocumentNode.SelectSingleNode("//div[@class='kjxq_box02_title_right']");
            var commentnode = footnode.SelectSingleNode("span[@class='iSelectBox']");
            var lastnode = commentnode.SelectSingleNode("//div[@class='iSelectList']");
            var anode = lastnode.SelectNodes("a"); //获取a标签里面的期号
            return anode;
        }
    }
}

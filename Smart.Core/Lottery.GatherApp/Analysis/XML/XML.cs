using Lottery.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        //读取xml
        public XmlNodeList GdklsfXml()
        {
            //辽宁快乐12  广东快乐十分  广西快乐10分 重庆时时彩 是网页版
            //gdklsf(广东快乐十分)  bjsyxw(北京11选5)  kl8(北京快乐8)   bjkzhc(北京快中彩)  bjpkshi(北京PK拾) bjk3(北京快3)  tjsyxw(天津11选5)
            //klsf(天津快乐十分)  tjssc(天津时时彩)  hebsyxw(河北11选5)  hebk3(河北快3)   nmgsyxw(内蒙古11选5)  nmgk3(内蒙古快3)  lnsyxw(辽宁11选5)
            //jlsyxw(吉林11选5)  jlk3(吉林快3)  hljsyxw(黑龙江11选5)  hljklsf(黑龙江快乐十分)   shhsyxw(上海11选5)  shhk3(上海快3)  ssl(上海时时乐) 
            //jssyxw(江苏11选5)  jsk3(江苏快3)  zjsyxw(浙江11选5)  zjkl12(浙江快乐12)  ahsyxw(安微11选5) ahk3(安微快三)  fjsyxw(福建11选5)
            //fjk3(福建快3)   dlc(江西11选5)  jxssc(江西时时彩) jxk3(江西快3)  qyh(山东群英会) shdsyxw(山东十一运夺冠)  shdklpk3(山东快乐扑克3)
            //hensyxw(河南11选5)  henk3(河南快3)  henky481(河南快赢481)  hbsyxw(湖北11选5)  hbk3(湖北快3)  hnklsf(湖南快乐十分) xysc(湖南快乐赛车)
            //gdsyxw(广东11选5)   gxsyxw(广西11选5)  gxk3(广西快3)  chqklsf(重庆快乐十分)  sckl12(四川快乐12)  gzsyxw(贵州11选5)  
            //gzk3(贵州快3)  sxsyxw(陕西11选5)  sxklsf(陕西快乐十分)  gssyxw(甘肃11选5)  gsk3(甘肃快3)  qhk3(青海快3)  xjsyxw(新疆11选5)
            //xjssc(新疆时时彩)   xjxlc(新疆喜乐彩)  shxsyxw(山西11选5)  ytdj(山西泳坛夺金) shxklsf(山西快乐十分)  ynsyxw(云南11选5)
            //ynklsf(云南快乐10分)  ynssc(云南时时彩)
            string htmlCode;
            var gameCode = "gdklsf";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");
            string Url = "http://kaijiang.500.com/static/info/kaijiang/xml/" + gameCode + "/" + date + ".xml";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/xml";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压  
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

            XmlNodeList list = doc.SelectNodes("//row");
           // _IXML_DataService.AddGdklsfAsync(list);
            return list;
            //foreach (XmlNode item in list)
            //{
            //    DataModel cust = new DataModel();
            //    cust.expect = item.Attributes["expect"].Value;
            //    cust.opencode = item.Attributes["opencode"].Value;
            //    cust.opentime = Convert.ToDateTime(item.Attributes["opentime"].Value);
            //    Console.WriteLine("开奖期号:" + cust.expect + "    开奖号码:" + cust.opencode + "   开奖时间:" + cust.opentime);
            //    lists.Add(cust);
            //}

        }
    }
}

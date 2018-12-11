using EntityModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace readXml
{
    class Program
    {
        static void Main(string[] args)
        {

            readXml();
            Console.ReadKey();
        }



        //读取xml
        public static void readXml()
        {
            //gdklsf(广东快乐十分)
            string htmlCode;
            var gameCode = "gdklsf";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");
            string Url = "http://kaijiang.500.com/static/info/kaijiang/xml/"+ gameCode + "/" + date + ".xml";
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
            List<DataModel> lists = new List<DataModel>();

            XmlNodeList list = doc.SelectNodes("//row");
            foreach (XmlNode item in list)
            {
                DataModel cust = new DataModel();
                cust.expect = item.Attributes["expect"].Value;
                cust.opencode = item.Attributes["opencode"].Value;
                cust.opentime = Convert.ToDateTime(item.Attributes["opentime"].Value);
                Console.WriteLine("开奖期号:" + cust.expect + "    开奖号码:" + cust.opencode + "   开奖时间:" + cust.opentime);
                lists.Add(cust);
            }

        }
    }
}

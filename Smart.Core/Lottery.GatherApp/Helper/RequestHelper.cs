using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Lottery.GatherApp
{
    public static class RequestHelper<T> where T : class, new()
    {
        public static readonly int TimeOut = 10000;
        /// <summary>
        /// 发起请求
        /// </summary>
        /// <param name="URI">目标URL,包含参数</param>
        /// <param name="requestType">请求类型(HTTP/HTTPS),enum RequestType</param>
        /// <param name="HeaderDic">头字典</param>
        /// <param name="UseIPAgent">是否使用IP代理</param>
        /// <param name="IP">代理IP</param>
        /// <param name="port">代理IP端口</param>
        /// <param name="UserAgent">用户代理</param>
        /// <param name="ContentType">文档请求类型</param>
        /// <param name="Method">请求方式Get/Post</param>
        /// <param name="parameters">Post参数</param>
        /// <returns></returns>
        public static T DoRequest(string URI, Dictionary<string, string> HeaderDic, bool UseIPAgent, string IP, string port, string UserAgent, string ContentType, Method method, IDictionary<string, string> parameters)
        {
            try
            {
                string StreamResult = string.Empty;
                var request = CreateRequest(URI, UserAgent, ContentType, method, out bool IsHttps);
                if (UseIPAgent)
                {
                    request.Proxy = IsUseAgent(IP, port, IsHttps);
                }
                if (HeaderDic != null)
                {
                    foreach (var item in HeaderDic)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.ContentEncoding != null && response.ContentEncoding.ToLower() == "gzip")
                {
                    using (Stream ResponseStream = response.GetResponseStream())
                    {
                        using (GZipStream gzipStream = new GZipStream(ResponseStream, CompressionMode.Decompress))
                        {
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            Encoding encoding = Encoding.GetEncoding("GB2312");
                            using (StreamReader StreamReader = new StreamReader(ResponseStream, encoding))
                            {
                                StreamResult = StreamReader.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (Stream ResponseStream = response.GetResponseStream())
                    {
                        using (StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.UTF8))
                        {
                            StreamResult = StreamReader.ReadToEnd();
                        }
                    }
                }
                T Tresult = default(T);
                if (typeof(HtmlDocument) == typeof(T))
                {
                    var DocResult = new HtmlDocument();
                    DocResult.LoadHtml(StreamResult);
                    return (T)(object)DocResult;
                }
                else if (typeof(XmlDocument) == typeof(T))
                {
                    var XMLResult = new XmlDocument();
                    XMLResult.LoadXml(StreamResult);
                    return (T)(object)XMLResult;
                }
                else if (typeof(String) == typeof(T) && ContentType.Contains("json"))
                {
                    return (T)(object)JsonConvert.SerializeObject(StreamResult);
                }
                else
                {
                    return (T)(object)StreamResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + ex.StackTrace);
            }
        }
        private static HttpWebRequest CreateRequest(string URI, string UserAgent, string ContentType, Method method,out bool ishttps)
        {
            ishttps = false;
            HttpWebRequest request;
            if (URI.ToLower().Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
                request = (HttpWebRequest)WebRequest.Create(URI);
                request.ProtocolVersion = HttpVersion.Version10;
                ishttps = true;
            }
            else
            {
                request=(HttpWebRequest)WebRequest.Create(URI);
            }
            request.Method = method == Method.Get ? "GET" : "POST";
            request.Timeout = TimeOut;
            request.UserAgent = UserAgent;
            request.ContentType = ContentType;
            request.AllowAutoRedirect = false;
            request.Accept = "*/*";
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.AllowWriteStreamBuffering = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;
            return request;
        }
        private static WebProxy IsUseAgent(string IP, string port,bool IsHttps)
        {
            WebProxy webProxy = null;
            if (CheckIPAgent(new WebProxy(IP, Convert.ToInt32(port)),IsHttps))
            {
                return new WebProxy(IP, Convert.ToInt32(port));
            }
            else
            {
                return null;
            }
        }
        public static bool CheckIPAgent(WebProxy web,bool IsHttps)
        {
            try
            {
                if (IsHttps)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sports.sina.com.cn/");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
    public static class GetEnumDesc
    {
        public static string GetDesc(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memberinfo = type.GetMember(en.ToString());
            if (memberinfo != null && memberinfo.Length > 0)
            {
                object[] attrs = memberinfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
    }
    public enum RequestType
    {
        HTTPS,
        HTTP
    }
    public enum Method
    {
        Post,
        Get,
    }
    public class ContentType
    {
        const string stream = "application/octet-stream";
        const string applicationX_001="application/x-001";
    }


}

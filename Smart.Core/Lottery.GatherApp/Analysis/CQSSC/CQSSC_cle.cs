using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class CQSSC_cle
    {

        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("CQSSC_cle");
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/xml");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                var result = client.GetAsync("http://www.cailele.com/static/ssc/newlyopenlist.xml").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    XDocument doc = XDocument.Parse(response);
                    var query = (from item in doc.Element("xml").Elements()
                                 select new
                                 {
                                     expect = item.Attribute("expect") == null ? (string)null : item.Attribute("expect").Value,
                                     opencode = item.Attribute("opencode") == null ? (string)null : item.Attribute("opencode").Value,
                                     opentime = item.Attribute("opentime") == null ? (string)null : item.Attribute("opentime").Value
                                 }).ToList();

                    int i = 0;
                    foreach (var item in query)
                    {
                        if (i == 5)
                            break;
                        list.Add($"{item.expect}|{item.opencode}");
                        i++;
                    }
                }
                return list;
            }
        }
    }
}

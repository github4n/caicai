using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class SD11X5_129kai
    {
        public static List<string> Analysis()
        {
            try
            {
                List<string> list = new List<string>();
                var client = NetHelper.GetHttpClient("SD11X5_129kai");
                {
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "zh-CN,zh;q=0.8");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "finecms_b1bf4_ci_session=a%3A5%3A%7Bs%3A10%3A%22session_id%22%3Bs%3A32%3A%22a150951e101a7a28f9c90a3270efb790%22%3Bs%3A10%3A%22ip_address%22%3Bs%3A15%3A%22125.227.136.181%22%3Bs%3A10%3A%22user_agent%22%3Bs%3A115%3A%22Mozilla%2F5.0+%28Windows+NT+10.0%3B+Win64%3B+x64%29+AppleWebKit%2F537.36+%28KHTML%2C+like+Gecko%29+Chrome%2F61.0.3163.100+Safari%2F537.36%22%3Bs%3A13%3A%22last_activity%22%3Bi%3A1510539736%3Bs%3A9%3A%22user_data%22%3Bs%3A0%3A%22%22%3B%7Dad9d7ada9f10e64113b3b8f9a550da65e0d5cd18");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                    ///client.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "https://129kai.net/index.php?c=api&a=updateinfo&cp=sd11x5&uptime=1488044922&chtime=37625&catid=188&modelid=25");
                    //client.DefaultRequestHeaders.TryAddWithoutValidation("Host", "129kai.net");
                    var result = client.GetAsync("http://129kai.net/index.php?c=api&a=updateinfo&cp=sd11x5&uptime=1488044922&chtime=37625&catid=188&modelid=25").Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                        var data = obj["list"];
                        int i = 0;
                        foreach (var item in data)
                        {
                            if (i == 5)
                                break;
                            var preDrawCode = item["c_r"].ToString();//开奖号
                            var preDrawIssue = item["c_t"].ToString();//开奖期号
                            list.Add($"{preDrawIssue}|{preDrawCode}");//开奖期号|开奖号
                            i++;
                        }
                        return list;
                    }
                    else
                    {
                        return list;
                    }

                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}

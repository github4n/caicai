using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
   public class BJPK10_129kai
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("BJPK10_129kai");
            {
                var result = client.GetAsync("https://129kai.net/index.php?c=api&a=updateinfo&cp=bjpk10&uptime=1488049174&chtime=33005&catid=2&modelid=9").Result;
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
                        //var preDrawTime = item["c_d"].ToString();//开奖时间
                        var preDrawIssue = item["c_t"].ToString();//开奖期号
                        //list.Add($"{preDrawCode}|{preDrawTime}|{preDrawIssue}");
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
    }
}

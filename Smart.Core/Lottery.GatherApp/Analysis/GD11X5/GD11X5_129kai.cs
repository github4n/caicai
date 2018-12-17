using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class GD11X5_129kai
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("GD11X5_129kai");
            {
                var result = client.GetAsync("http://129kai.net/index.php?c=api&a=updateinfo&cp=gd11x5&uptime=1488043311&chtime=36485&catid=125&modelid=16").Result;
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
    }
}

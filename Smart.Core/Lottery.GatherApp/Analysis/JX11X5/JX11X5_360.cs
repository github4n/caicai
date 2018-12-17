using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class JX11X5_360
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("JX11X5_360");
            {
                var result = client.GetAsync("http://chart.cp.360.cn/zst/qkj/?lotId=168009").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    var preDrawIssue = obj["0"]["Issue"].ToString();//开奖期号
                    var preDrawCode = obj["0"]["WinNumber"].ToString().Trim();//开奖号
                    var preDrawTime = obj["0"]["EndTime"].ToString();//开奖时间
                    if (preDrawCode.Split(' ').Length == 5)
                    {
                        preDrawCode = preDrawCode.Replace(" ", ",");
                    }
                    list.Add($"{preDrawIssue}|{preDrawCode}");
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

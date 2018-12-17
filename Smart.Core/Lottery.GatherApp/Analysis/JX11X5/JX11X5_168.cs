using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class JX11X5_168
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("JX11X5_168");
            {
                var result = client.GetAsync("http://api.api68.com/ElevenFive/getElevenFiveList.do?date=&lotCode=10015").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    string businessCode = obj["result"]["businessCode"].ToString();
                    var data = obj["result"]["data"];
                    int i = 0;
                    foreach (var item in data)
                    {
                        if (i == 5)
                            break;
                        string preDrawCode = item["preDrawCode"].ToString().Trim();//开奖号
                        string preDrawTime = item["preDrawTime"].ToString();//开奖时间
                        string preDrawIssue = item["preDrawIssue"].ToString();//开奖期号
                        if (preDrawCode.Split(' ').Length == 5)
                        {
                            preDrawCode = preDrawCode.Replace(" ", ",");
                        }
                        list.Add($"{preDrawIssue}|{preDrawCode}");
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

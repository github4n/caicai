using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class BJKL8_168
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("BJKL8_168");
            {
                var result = client.GetAsync("http://api.api68.com/LuckTwenty/getBaseLuckTwentyList.do?date=&lotCode=10014").Result;
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
                        string preDrawCode = item["preDrawCode"].ToString();//开奖号
                        string preDrawTime = item["preDrawTime"].ToString();//开奖时间
                        string preDrawIssue = item["preDrawIssue"].ToString();//开奖期号
                        var arr = preDrawCode.Split(',').ToList();
                        if (arr.Count == 21)
                        {
                            var first = preDrawCode.Substring(0, preDrawCode.LastIndexOf(","));
                            var second = preDrawCode.Substring(preDrawCode.LastIndexOf(",") + 1);
                            preDrawCode = $"{first}+{second}";
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class FC3D_168
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("FC3D_168");
            {
                var result = client.GetAsync("http://api.api68.com/QuanGuoCai/getLotteryInfoList.do?lotCode=10041").Result;
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class JSK3_opencai
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("JSK3_opencai");
            {
                var result = client.GetAsync("http://t.apiplus.net/newly.do?code=jsk3&format=json").Result;
                if (result.IsSuccessStatusCode)
                {

                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    var data = obj["data"];
                    foreach (var item in data)
                    {
                        var preDrawCode = item["opencode"].ToString();//开奖号
                        //var preDrawTime = item["opentime"].ToString();//开奖时间
                        var preDrawIssue = item["expect"].ToString();//开奖期号
                        //list.Add($"{preDrawCode}|{preDrawTime}|{preDrawIssue}");
                        list.Add($"{preDrawIssue}|{preDrawCode}");
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

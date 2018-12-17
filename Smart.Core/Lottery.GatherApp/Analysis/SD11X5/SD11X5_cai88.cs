using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
   public class SD11X5_cai88
    {
        /// <summary>
        /// 开奖是2017/10/19 17:37:10不是正数10
        /// </summary>
        /// <returns></returns>
        public List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("SD11X5_cai88");
            {
                var result = client.GetAsync("http://cai88.com/api/getgame.action?type=107").Result;
                if (result.IsSuccessStatusCode)
                {
                   
                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    var data1 = obj["model"]["entity"]["start"].ToString();
                    var data2 = obj["model"]["entity"]["end"].ToString();
                    var data = obj["model"]["entity"]["list"];
                    foreach (var item in data)
                    {
                        var preDrawCode = item["code"].ToString();//开奖号
                        var preDrawTime = data1;//开奖时间
                        var preDrawIssue = item["issue"].ToString();//开奖期号
                        list.Add($"{preDrawCode}|{preDrawTime}|{preDrawIssue}");
                        break;
                    }
                }
                return list;
            }
        }
    }
}

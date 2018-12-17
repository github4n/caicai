﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class JSK3_360
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("JSK3_360");
            {
                var result = client.GetAsync("http://chart.cp.360.cn/zst/qkj/?lotId=255903").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    var preDrawIssue = obj["0"]["Issue"].ToString();//开奖期号
                    var preDrawCode = obj["0"]["WinNumber"].ToString();//开奖号
                    var preDrawTime = obj["0"]["EndTime"].ToString();//开奖时间
                    preDrawCode = string.Join(",", preDrawCode.ToCharArray());
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

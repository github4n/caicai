using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Smart.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.GatherApp.Analysis.HK6
{
    public class HK6_168
    {
        /// <summary>
        /// https://1680660.com/smallSix/queryLotteryDate.do?ym=2018-12 
        /// {"message":"操作成功","result":{"businessCode":0,"message":"操作成功","data":{"preDrawDate":"2018-12","kjDate":[[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[1,1],[2,0],[3,0],[4,1],[5,0],[6,1],[7,0],[8,1],[9,0],[10,0],[11,1],[12,0],[13,1],[14,0],[15,1],[16,0],[17,0],[18,1],[19,0],[20,1],[21,0],[22,1],[23,0],[24,0],[25,1],[26,0],[27,1],[28,0],[29,0],[30,1],[31,0],[0,0],[0,0],[0,0],[0,0],[0,0]],"id":345}},"errorCode":0}
        /// 
        /// https://1680660.com/smallSix/findSmallSixHistory.do?year=2018&type=1
        /// </summary>
        /// <returns></returns>
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("HK6_168");
            {
                var result = client.GetAsync("https://1680660.com/smallSix/findSmallSixHistory.do?year=2018&type=1").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                    string businessCode = obj["result"]["data"]["bodyList"].ToString();
                    var data = obj["result"]["data"]["bodyList"];
                    int i = 0;
                    foreach (var item in data)
                    {
                        if (i == 5)
                            break;
                        string preDrawCode = item["preDrawCode"].ToString();//开奖号
                        string preDrawTime = item["preDrawDate"].ToString();//开奖时间
                        string preDrawIssue = item["issue"].ToString();//开奖期号
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


        public static List<string> AnalysisIssue()
        {
            //var startDate = DateTime.Now.ToString("yyyy-MM");
            //var endDate = DateTime.Now.AddMonths(12).ToString("yyyy-MM");
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("HK6_168");
            {
                for (int i = 0; i < 12; i++)
                {
                    var startDate = DateTime.Now.AddMonths(i).ToString("yyyy-MM");
                    var result = client.GetAsync($"https://1680660.com/smallSix/queryLotteryDate.do?ym={startDate}").Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = result.Content.ReadAsStringAsync().Result;
                        JObject obj = (JObject)JsonConvert.DeserializeObject(response);
                        string message = obj["result"]["message"].ToString();
                        if (message != "操作成功")
                            return list;
                        string businessCode = obj["result"]["data"]["preDrawDate"].ToString();
                        var data = obj["result"]["data"]["kjDate"];
                        foreach (var item in data)
                        {
                            var flag1 = item[0].ToString();
                            var flag2 = item[1].ToString();
                            if (flag2 == "0")
                                continue;
                            list.Add($"{startDate}-{flag1}");
                        }
                    }
                }
                
            }

            return list;
        }
    }
}

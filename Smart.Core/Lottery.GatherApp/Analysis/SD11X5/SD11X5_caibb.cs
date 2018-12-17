using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class SD11X5_caibb
    {
        /// <summary>
        /// 开奖是2017/10/19 17:37:10不是正数10
        /// </summary>
        /// <returns></returns>
        public static List<string> Analysis(string issue)
        {
            var client = NetHelper.GetHttpClient("SD11X5_cai88");
            {
                issue = issue.Insert(8,"-");
                List<string> list = new List<string>();
                var result = client.GetAsync($"https://www.caibb.com//buy/awardlist/sd11x5?isue={issue}&pageNo=0&pageSize=10").Result;
                if (result.IsSuccessStatusCode)
                {

                    var response = result.Content.ReadAsStringAsync().Result;
                    string pattern = "<span class=\"r_border\">(?<preDrawIssue>.*?)</span>.*?<span class=\"qihao\">\n.*?(?<preDrawCode>.*?)</span>";
                    var matchs = Regex.Matches(response, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match item in matchs)
                    {
                        var preDrawIssue = item.Groups["preDrawIssue"].Value;
                        var preDrawCode = item.Groups["preDrawCode"].Value.Trim();
                        list.Add($"{preDrawIssue}|{preDrawCode}");//开奖期号|开奖号
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

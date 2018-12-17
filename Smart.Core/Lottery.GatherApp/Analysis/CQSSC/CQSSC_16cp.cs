using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class CQSSC_16cp
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("CQSSC_16cp");
            {
                var result = client.GetAsync("http://www.16cp.com/Game/GetNum.aspx?iType=3").Result;
                if (result.IsSuccessStatusCode)
                {
                    byte[] data = result.Content.ReadAsByteArrayAsync().Result;
                    var response = GetString(data);
                    var matches = Regex.Matches(response, "<ul.*?><li.*?>(?<value1>.*?)</li><li.*?>(?<value2>.*?)</li><li.*?>(?<value3>.*?)</li></ul>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    int i = 0;
                    foreach (Match item in matches)
                    {
                        if (i == 5)
                            break;
                        var preDrawCode = item.Groups["value2"].ToString();//开奖号
                        //var preDrawTime = $"{DateTime.Now.Year}-{item.Groups["value3"].ToString()}";//开奖时间
                        var preDrawIssue = item.Groups["value1"].ToString();//开奖期号
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


        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str.ToCharArray());
        }

        public static string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

    }
}

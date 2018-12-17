using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Smart.Core.Utils;

namespace Lottery.GatherApp
{
    public class BJPK10_bwlc
    {
        public static List<string> Analysis()
        {
            List<string> list = new List<string>();
            var client = NetHelper.GetHttpClient("BJPK10_bwlc");
            {
                var result = client.GetAsync("http://www.bwlc.gov.cn/bulletin/trax.html").Result;
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    string regex = "<table class=\"tb\".*?>(?<content>.*?)</table>";
                    var match = Regex.Match(response, regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (!match.Success)
                        return null;
                    string context = match.Groups["content"].Value;//<tr class=.*>.*?<td>(?<isses>.*?)</td>.*?<td>(?<code>.*?)</td>.*?<td>(?<code1>.*?)</td>.*?<td>.*?</td>.*?</tr>
                    var matchs = Regex.Matches(context, "<td>(?<isses>.*?)</td>.*?<td>(?<code>.*?)</td>.*?<td>(?<code1>.*?)</td>.*?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    int i = 0;
                    foreach (Match mc in matchs)
                    {
                        if (i == 5)
                            break;
                        var isses = mc.Groups["isses"].Value;
                        var code = mc.Groups["code"].Value;
                        list.Add($"{isses}|{code}");
                        i++;
                    }
                }
            }


            return list;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lottery.GatherApp
{
    public class AMPK10_my
    {
        public static string Analysis(int len = 10)
        {
            string code = string.Empty;
            Random r = new Random();
            int[] nums = Enumerable.Range(1, len)
                .OrderBy(x => r.Next()).ToArray();
            foreach (int n in nums)
                code += n + ",";
            return code.TrimEnd(',');
        }
    }
}

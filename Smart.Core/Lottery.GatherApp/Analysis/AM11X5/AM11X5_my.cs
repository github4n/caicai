using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lottery.GatherApp
{
    public class AM11X5_my
    {
        public static string Analysis(int len = 5)
        {
            var arr = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11" };
            string code = string.Empty;
            Hashtable hashtable = new Hashtable();
            Random random = new Random();
            for (int i = 0; hashtable.Count < len; i++)
            {
                int nValue = random.Next(arr.Length - 1);
                if (!hashtable.ContainsValue(nValue))
                {
                    code += arr[nValue] + ",";
                    hashtable.Add(nValue, nValue);
                }
            }
            return code.TrimEnd(',');
        }

    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lottery.GatherApp
{
    public class AMKL8_my
    {
        public static string Analysis(int len = 20)
        {
            var arr = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36",
            "37","38","39","40","41","42","43","44","45","46","47","48","49","50","51","52","53","54","55","56","57","58","59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77","78","79","80"};
            string code = string.Empty;
            Hashtable hashtable = new Hashtable();
            Random random = new Random();
            for (int i = 0; hashtable.Count < len; i++)
            {
                int nValue = random.Next(arr.Length);
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

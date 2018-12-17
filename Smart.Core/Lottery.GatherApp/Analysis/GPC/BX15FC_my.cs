using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.GatherApp
{
    public class BX15FC_my
    {
        public static string Analysis(int len = 5)
        {
            string code = string.Empty;
            for (int i = 0; i < len; i++)
            {
                Random random = new Random();
                code += random.Next(0, 9) + ",";
            }
            return code.TrimEnd(',');
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.GatherApp
{
    public class AM3D_my
    {
        public static string Analysis(int len = 3)
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


using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Extensions
{
    public static class StringHelper
    {
        /// <summary>
        /// 返回"yyyy-MM-dd"
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static string FormatDate(this string st)
        {
            if (string.IsNullOrEmpty(st)) return st;
            if (st.Length >= 10)
            {
                return st.Substring(0, 10);
            }
            return st;
        }
    }
}

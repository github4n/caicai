using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Smart.Core.Utils
{
    public static class XSSHelper
    {
        /// <summary>
        /// XSS过滤
        /// </summary>
        /// <param name="html">html代码</param>
        /// <returns>过滤结果</returns>
        public static string XssFilter(string html)
        {
            string str = HtmlFilter(html);
            return str;
        }

        /// <summary>
        /// 过滤HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HtmlFilter(string Htmlstring)
        {
            // 写自己的处理逻辑即可，下面给出一个比较暴力的孤哦旅，把 匹配到<[^>]*>全部过滤掉，建议慎用，只是一个例子
            string result = Regex.Replace(Htmlstring, @"<[^>]*>", String.Empty);
            return result;
        }
    }
}

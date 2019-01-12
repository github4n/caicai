using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;

namespace Smart.Core.Utils
{
    public class CommonHelper
    {
        public const string GlobalApiKey = "global";
        public const string HeaderStatusKey = "Api-Throttle-Status";
        public static string IpToNum(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return ip;

            IPAddress ipAddr = IPAddress.Parse(ip);
            List<Byte> ipFormat = ipAddr.GetAddressBytes().ToList();
            ipFormat.Reverse();
            ipFormat.Add(0);
            BigInteger ipAsInt = new BigInteger(ipFormat.ToArray());
            return ipAsInt.ToString();
        }


        public enum CollectionUrlEnum
        {
            /// <summary>
            /// 未知
            /// </summary>
            url_unknow = 0,
            /// <summary>
            /// 500网开奖主站
            /// </summary>
            url_500kaijiang = 1,
            /// <summary>
            /// 500网足彩相关
            /// </summary>
            url_500zx = 2,
            /// <summary>
            /// jdd采集
            /// </summary>
            url_jdd = 3,
            /// <summary>
            /// 1122开奖网
            /// </summary>
            url_1122 = 4,
            /// <summary>
            /// 彩客
            /// </summary>
            url_caike = 5
        }

    }
}

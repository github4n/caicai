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
    }
}

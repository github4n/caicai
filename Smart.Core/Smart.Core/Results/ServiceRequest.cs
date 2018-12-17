using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Results
{
    public class ServiceRequest
    {
        /// <summary>
        /// 请求event
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// 请求来源
        /// 网站投注=100
        /// iPhone客户端投注=101
        /// Android客户端投注=102
        /// WAP网站投注=103
        /// 触屏版投注=104
        /// 后台管理=105
        /// </summary>
        public int source_code { get; set; }
        /// <summary>
        /// 业务参数
        /// </summary>
        public object param { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 加密串
        /// </summary>
        public string sign { get; set; }

        public override string ToString()
        {
            return $"managerxingcai{date}{source_code}{action}";
        }
    }
}

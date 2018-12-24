using Smart.Core.Throttle;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace EntityModel.Model
{
   public class LotteryServiceRequest
    {
        ///// <summary>
        ///// 请求来源
        ///// </summary>
        //public SchemeSource SourceCode { get; set; }     
        ///// <summary>
        ///// 消息序号（服务器原样返回）
        ///// </summary>
        //public string MsgId { get; set; }
        /// <summary>
        /// 业务参数
        /// </summary>
        public string Param { get; set; }
        

        public string TimeStamp { get; set; }
    }
}

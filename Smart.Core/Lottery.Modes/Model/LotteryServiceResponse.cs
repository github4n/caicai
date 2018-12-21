using Smart.Core.Throttle;
using System.Text;

namespace EntityModel.Model
{
    public class LotteryServiceResponse
    {
        /// <summary>
        /// 消息序号（与传入时一样）
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 结果码
        /// </summary>
        public ResponseCode Code { get; set; }
        /// <summary>
        /// 处理提示消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        public object Value { get; set; }
    }
}

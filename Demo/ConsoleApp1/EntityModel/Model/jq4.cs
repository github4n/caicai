using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
     public class jq4
    {
        public jq4() {

            openTeam = new List<jq4Team>();
            openCode = new List<jq4Core>();
            openLotteryDetails = new List<jq4LotteryDetails>();
         
        
        }
        public string Id { get; set; }

        /// <summary>
        /// 进球彩期号
        /// </summary>
        public string expect { get; set; }
        /// <summary>
        /// 球队
        /// </summary>
        public List<jq4Team> openTeam { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public List<jq4Core> openCode { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string openTime { get; set; }

        /// <summary>
        /// 兑奖截止日期
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public string SalesVolume { get; set; }
        /// <summary>
        /// 奖池滚存
        /// </summary>
        public string PoolRolling { get; set; }

        /// <summary>
        /// 开奖详情
        /// </summary>
        public List<jq4LotteryDetails> openLotteryDetails { get; set; }
      
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class dlt
    {
        public string Id { get; set; }

        /// <summary>
        /// 进球彩期号
        /// </summary>
        public string Expect { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenCode { get; set; }

        /// <summary>
        /// 出球顺序
        /// </summary>
        public string OutOfOrder { get; set; }


        /// <summary>
        /// 销量
        /// </summary>
        public string SalesVolume { get; set; }
        /// <summary>
        /// 奖池滚存
        /// </summary>
        public string PoolRolling { get; set; }



    }
}

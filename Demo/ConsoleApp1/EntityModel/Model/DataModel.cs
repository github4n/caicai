using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModel.Model
{
   public class DataModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public string expect { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string opencode { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime opentime { get; set; }

    }


}

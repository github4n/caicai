using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    // 
    ///</summary>

    public class normal_lotterydetail : EntityBase
    { 
        public normal_lotterydetail()
        {
        
        }
        /// <summary>
        /// 
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public int LotteryId{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryCode{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string IssueNo{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime CreateTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string OpenTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string AwardDeadlineTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string PrizePool{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryDataDetail{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryResultDetail{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public int? Sys_IssueId{ get; set; }
    }
}

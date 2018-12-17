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

    public class sys_issue : EntityBase
    { 
        public sys_issue()
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
         
            public string IssueNo{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public int? LotteryId{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryCode{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime CreateTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? UpdateTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string OpenTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string OpenCode{ get; set; }
    }
}

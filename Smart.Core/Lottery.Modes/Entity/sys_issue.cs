using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    ///期号表
    ///</summary>

    public class sys_issue : EntityBase
    { 
        public sys_issue()
        {
        
        }
        /// <summary>
        /// 期号id
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id{ get; set; }
        /// <summary>
        /// 期号名称
        ///</summary>

        public string IssueNo{ get; set; }
        /// <summary>
        /// 彩种id
        ///</summary>

        public int? LotteryId{ get; set; }
        /// <summary>
        /// 彩种码
        ///</summary>

        public string LotteryCode{ get; set; }
        /// <summary>
        /// 创建时间
        ///</summary>

        public DateTime CreateTime{ get; set; }
        /// <summary>
        /// 更新时间
        ///</summary>

        public DateTime? UpdateTime{ get; set; }
        /// <summary>
        /// 开奖时间
        ///</summary>

        public string OpenTime{ get; set; }
        /// <summary>
        /// 开奖号码
        ///</summary>

        public string OpenCode{ get; set; }
    }
}

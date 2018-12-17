using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace Lottery.Modes.Entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class blast_count: EntityBase
    {
           public blast_count(){


           }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>    
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id {get;set;}

           /// <summary>
           /// Desc:彩种ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int typeid {get;set;}

           /// <summary>
           /// Desc:玩法ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int playedId { get;set;}

           /// <summary>
           /// Desc:统计日期,一天一次
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime createdate { get;set;}

           /// <summary>
           /// Desc:投注数
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int betCount { get;set;}

           /// <summary>
           /// Desc:投注金额
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal betAmount { get;set;}

           /// <summary>
           /// Desc:中奖金额
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal zjAmount { get;set;}

    }
}

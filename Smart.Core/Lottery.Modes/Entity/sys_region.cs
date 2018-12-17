using Smart.Core.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    ///地区表
    ///</summary>

    public class sys_region : EntityBase
    { 
        public sys_region()
        {
        
        }
        /// <summary>
        /// 地区主键编号
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Region_Id{ get; set; }
        /// <summary>
        /// 地区名称
        ///</summary>

        public string RegionName{ get; set; }
        /// <summary>
        /// 是否有高频彩
        ///</summary>

        public bool IsGPC{ get; set; }
        /// <summary>
        /// 是否有地方彩
        ///</summary>

        public bool IsDFC{ get; set; }
        /// <summary>
        /// 是否显示
        ///</summary>

        public bool IsShow{ get; set; }
    }
}

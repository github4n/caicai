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

    public class sys_region : EntityBase
    { 
        public sys_region()
        {
        
        }
        /// <summary>
        /// 
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Region_Id{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string RegionName{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public bool IsGPC{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public bool IsDFC{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public bool IsShow{ get; set; }
    }
}

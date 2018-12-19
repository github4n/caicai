using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    // 
    ///</summary>

    public class sys_lottery
    { 
        public sys_lottery()
        {
        
        }
            /// <summary>
            /// 
            ///</summary>
          
            public int Lottery_Id{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryName{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryCode{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string RegionName { get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryDay{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string NumberPeriods{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LotteryFrequency{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public int? HighFrequency{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? CreateTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? UpdateTime{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public bool IsShow{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string WebLogo{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string WapLogo{ get; set; }
    }
}

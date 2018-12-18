using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lottery.Modes.Entity
{
    /// <summary>
    /// 
    ///</summary>

    public class zqdc_sfgg_result
    { 
        public zqdc_sfgg_result()
        {
        
        }
        /// <summary>
        /// 
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string MatchId{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string MatchDate{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string MatchNumber{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string BallType{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string BallType_Color{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public bool? IsFinish{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string HomeTeam{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string GuestTeam{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LetBall{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SPF_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string AvgEu_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string IssueNo{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? CreateTime{ get; set; }
    }
}

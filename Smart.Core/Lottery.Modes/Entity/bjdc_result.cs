using Smart.Core.Repository;
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

    public class bjdc_result: EntityBase
    { 
        public bjdc_result()
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
         
            public string HalfScore{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string FullScore{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string LeagueName{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string League_Color{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SPF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SPF_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string RQSPF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string RQSPF_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string ZJQ_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string ZJQ_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string BF_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string BQC_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string BQC_SP{ get; set; }
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
         
            public bool IsFinish{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SXDS_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SXDS_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? CreateTime{ get; set; }
    }
}

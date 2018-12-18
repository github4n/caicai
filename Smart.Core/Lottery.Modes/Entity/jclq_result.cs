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

    public class jclq_result
    { 
        public jclq_result()
        {
        
        }
        /// <summary>
        /// 
        ///</summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
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
         
            public string SF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string GG_SF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string RFSF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string GG_RFSF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string SFC_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string GG_SFC_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string YSZF{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string DXF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string GG_DXF_Result{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string AvgEu_SP{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public string JCDate{ get; set; }
            /// <summary>
            /// 
            ///</summary>
         
            public DateTime? CreateTime{ get; set; }
    }
}

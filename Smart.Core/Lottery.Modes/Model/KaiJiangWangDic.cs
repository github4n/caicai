using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
    public static class KaiJiangWangDic
    {
        public static readonly Dictionary<string, string> GaoPindic = new Dictionary<string, string>() { };
        public static readonly Dictionary<string, string> QuanguoDic = new Dictionary<string, string>() { };
        public static readonly Dictionary<string, int> RedBallGameCode = new Dictionary<string, int>();
        static KaiJiangWangDic()
        {
            GaoPindic.Add("gdklsf", "kuai10/10301");
            GaoPindic.Add("gdsyxw", "selected5/10201");
            GaoPindic.Add("shhsyxw", "selected5/10203");
            GaoPindic.Add("ahsyxw", "selected5/10204");
            GaoPindic.Add("dlc", "selected5/10205");
            GaoPindic.Add("jlsyxw", "selected5/10206");
            GaoPindic.Add("gxsyxw", "selected5/10207");
            GaoPindic.Add("hbsyxw", "selected5/10208");
            GaoPindic.Add("lnsyxw", "selected5/10209");
            GaoPindic.Add("zjsyxw", "selected5/10211");
            GaoPindic.Add("nmgsyxw", "selected5/10212");
            GaoPindic.Add("jsk3", "kuai3/10401");
            GaoPindic.Add("jlk3", "kuai3/10402");
            GaoPindic.Add("hebk3", "kuai3/10403");
            GaoPindic.Add("ahk3", "kuai3/10404");
            GaoPindic.Add("fjk3", "kuai3/10406");
            GaoPindic.Add("hbk3", "kuai3/10407");
            GaoPindic.Add("bjk3", "kuai3/10408");
            GaoPindic.Add("gxk3", "kuai3/10409");
            GaoPindic.Add("ssc", "sscs/10101");
            GaoPindic.Add("tjssc", "sscs/10103");

            QuanguoDic.Add("ssq", "10801");
            QuanguoDic.Add("sd", "10802");
            QuanguoDic.Add("qlc", "10803");
            QuanguoDic.Add("dlt", "10804");
            QuanguoDic.Add("pls", "10805");
            QuanguoDic.Add("plw", "10806");

            RedBallGameCode.Add("ssq",1);
            RedBallGameCode.Add("qlc",1);
            RedBallGameCode.Add("dlt",2);
        }
    }
}

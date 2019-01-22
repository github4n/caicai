using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;
using Lottery.Modes.OtherModel;

namespace Lottery.Services.Abstractions
{
   public interface ISport_DataService
    {
        void Add_BJDC(List<jczq> model,string GameCode= "zqdc");
        void Add_JCLQ(List<jclq_result> model, string GameCode= "jclq");
        void Add_JCZQ(List<jczq> model,string GameCode= "jczq");
        Dictionary<string, string> GetNotFinish(string GameCode);
        List<string> GetNow3IssuNo(string GameCode);
        string GetNowIssuNo(string GameCode);
        string GetJCZQ_JCDate();
        string GetJCLQ_JCDate();
        int AddCaiKeJCLQ(Caike_Body caike_Body,string matchDateCode, DateTime dateTime);
        int AddCaikeJCZQ(Caike_Body caike_Body, string matchDateCode, DateTime dateTime);
        int AddCaiKeBJDC(Caike_Body caike_Body, string matchDateCode);
        List<int> GetIssuNoList(string GameCode);
        int GetIssueInResult();
    }
}

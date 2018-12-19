using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;

namespace Lottery.Services.Abstractions
{
   public interface ISport_DataService
    {
        void Add_BJDC(List<jczq> model,string GameCode= "zqdc");
        void Add_JCLQ(List<jclq_result> model, string GameCode= "jclq");
        void Add_JCZQ(List<jczq> model,string GameCode= "jczq");
        List<string> GetNotFinish(string GameCode);
        List<string> GetNow3IssuNo(string GameCode);
        string GetNowIssuNo(string GameCode);
        string GetJCZQ_JCDate();
        string GetJCLQ_JCDate();
    }
}

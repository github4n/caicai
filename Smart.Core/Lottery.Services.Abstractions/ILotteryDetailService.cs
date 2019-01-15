using Lottery.Modes.Entity;
using Lottery.Modes.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Abstractions
{
    public interface ILotteryDetailService
    {
        Task<int> AddLotteryDetal(List<lotterydetail> lotterydetails, string gameCode);
        List<sys_issue> GetLotteryCodeList(string LotteryCode);

        normal_lotterydetail GetNowIssuNo(string LotteryCode);

        sys_issue GetIssue(string IssueNo);
        normal_lotterydetail GetCodelotterydetail(string LotteryCode, string IssueNo);
    }
}

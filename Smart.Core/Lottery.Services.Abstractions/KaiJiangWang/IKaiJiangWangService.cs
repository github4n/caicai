using Lottery.Modes;
using Lottery.Modes.Entity;

namespace Lottery.Services.Abstractions
{
    public interface IKaiJiangWangService
    {
        sys_issue GetIssue(string lotteryCode);
        int GetLottery(string lotteryCode);
        void AddSys_issue(string LotteryCode, JsonReuslt jsonList);
    }
}

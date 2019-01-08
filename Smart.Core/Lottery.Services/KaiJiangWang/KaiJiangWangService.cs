using System;
using System.Collections.Generic;
using System.Text;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using Lottery.Modes.Entity;
using Lottery.Modes;
using Lottery.Modes.Model;
using System.Linq;

namespace Lottery.Services
{
   public class KaiJiangWangService: Repository<DbFactory>, IKaiJiangWangService
    {
        protected SqlSugarClient db = null;

        public KaiJiangWangService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }
        public sys_issue GetIssue(string lotteryCode)
        {
            return db.Queryable<sys_issue>().Where(x => x.LotteryCode == lotteryCode).OrderBy(x => x.IssueNo, OrderByType.Desc).First();
        }
        public int GetLottery(string lotteryCode)
        {
            return db.Queryable<sys_lottery>().Where(x => x.LotteryCode == lotteryCode).Select(x=>x.Lottery_Id).First();
        }
        public void AddSys_issue(string LotteryCode, JsonReuslt jsonList)
        {
            try
            {
                var LotteryId = GetLottery(LotteryCode);
                jsonList.content.ForEach((a) =>
                {
                    sys_issue _Issue = new sys_issue();
                    if (KaiJiangWangDic.RedBallGameCode.ContainsKey(LotteryCode))
                    {
                        var dic = KaiJiangWangDic.RedBallGameCode.Where(x=>x.Key== LotteryCode).FirstOrDefault();
                        int i = 0;
                        a.preDrawCode.ForEach((x) =>
                        {
                            if (a.preDrawCode.Count - i == dic.Value)
                            {
                                _Issue.OpenCode = _Issue.OpenCode.Remove(_Issue.OpenCode.Length - 1, 1);
                                _Issue.OpenCode = _Issue.OpenCode + "|" + x;
                            }
                            else
                            {
                                _Issue.OpenCode = _Issue.OpenCode + x + ",";
                            }
                            i++;
                        });
                    }
                    else
                    {
                        a.preDrawCode.ForEach((x) =>
                        {
                            _Issue.OpenCode = _Issue.OpenCode + x + ",";
                        });
                        _Issue.OpenCode = _Issue.OpenCode.Remove(_Issue.OpenCode.Length - 1, 1);
                    }
                    _Issue.IssueNo = a.preDrawIssue.ToString().Length==5? a.preDrawIssue.ToString(): a.preDrawIssue.ToString().Substring(0,2);
                    _Issue.LotteryId = LotteryId;
                    _Issue.LotteryCode = LotteryCode;
                    _Issue.CreateTime = DateTime.Now;
                    _Issue.OpenTime = a.preDrawTime;
                    _Issue.LotteryTime = Convert.ToDateTime(a.preDrawTime).ToString("yyyy-MM-dd");
                    var model = db.Queryable<sys_issue>().First(x => x.LotteryCode == LotteryCode && x.IssueNo == _Issue.IssueNo);
                    if (model == null)
                    {
                        db.Insertable<sys_issue>(_Issue).ExecuteCommand();
                        Console.WriteLine($"{LotteryCode}奖期{_Issue.IssueNo}成功新增数据");
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + ex.StackTrace);
            }
        }
    }
}

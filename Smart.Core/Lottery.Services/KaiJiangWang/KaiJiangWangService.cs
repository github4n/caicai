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
            var LotteryId = GetLottery(LotteryCode);
            jsonList.content.ForEach((a) =>
            {
                try
                {
                    sys_issue _Issue = new sys_issue();
                    if (KaiJiangWangDic.RedBallGameCode.ContainsKey(LotteryCode))
                    {
                        var dic = KaiJiangWangDic.RedBallGameCode.Where(x => x.Key == LotteryCode).FirstOrDefault();
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
                    var IssueNo = a.preDrawIssue.ToString();
                    if (KaiJiangWangDic.CutStartIndex.ContainsKey(LotteryCode))
                    {
                        var CutDic = KaiJiangWangDic.CutStartIndex.Where(x => x.Key == LotteryCode).FirstOrDefault();
                        IssueNo = IssueNo.Substring(CutDic.Value);
                    }
                    if (KaiJiangWangDic.AddStartChar.ContainsKey(LotteryCode))
                    {
                        var AddDic = KaiJiangWangDic.AddStartChar.Where(x => x.Key == LotteryCode).FirstOrDefault();
                        IssueNo = AddDic.Value + IssueNo;
                    }
                    if (KaiJiangWangDic.AddChar.ContainsKey(LotteryCode))
                    {
                        var CharDic = KaiJiangWangDic.AddChar.Where(x => x.Key == LotteryCode).FirstOrDefault();
                        IssueNo = IssueNo.Insert(IssueNo.Length - CharDic.Value, "-");
                    }
                    if (KaiJiangWangDic.DeleteZero.ContainsKey(LotteryCode))
                    {
                        var CharDic = KaiJiangWangDic.DeleteZero.Where(x => x.Key == LotteryCode).FirstOrDefault();

                        //if (IssueNo.StartsWith("20") && IssueNo.Length == 11)//20181224080
                        //{
                        //    IssueNo = IssueNo.Remove(CharDic.Value, 1);
                        //}
                        //else if (IssueNo.Length == 9 && !IssueNo.StartsWith("20"))//181224080
                        //{
                            IssueNo = IssueNo.Remove(CharDic.Value, 1);
                       // }
                    }
                    _Issue.IssueNo = IssueNo;
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
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message+":"+ex.StackTrace);
                }
            });
        }
    }
}

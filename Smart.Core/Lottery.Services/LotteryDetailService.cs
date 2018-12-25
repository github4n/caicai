using Lottery.Modes.Entity;
using Lottery.Modes.Model;
using Lottery.Services.Abstractions;
using Newtonsoft.Json;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services
{
    public class LotteryDetailService : Repository<DbFactory>, ILotteryDetailService
    {
        protected SqlSugarClient db = null;
        public LotteryDetailService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }

        public async Task<int> AddLotteryDetal(List<lotterydetail> lotterydetails,string gameCode)
        {
            int count = 0;
            int insertCount = 0;
            var LotteryCode = db.Queryable<sys_lottery>().Where(x => x.LotteryCode == gameCode).First();
            normal_lotterydetail nld = GetNowIssuNo(gameCode);
            foreach (var item in lotterydetails)
            {
                if (nld != null)
                {
                    if (nld.IssueNo == item.expect)
                    {
                        break;
                    }
                }
                normal_lotterydetail lotterydetail = new normal_lotterydetail();
                lotterydetail.IssueNo = item.expect;
                lotterydetail.LotteryId = LotteryCode.Lottery_Id;
                lotterydetail.LotteryCode = LotteryCode.LotteryCode;
                lotterydetail.OpenTime = item.openTime;
                lotterydetail.AwardDeadlineTime = item.endTime;
                lotterydetail.LotteryDataDetail = item.openCode+"|"+item.NumberType;
                lotterydetail.CurrentSales = item.SalesVolume;
                lotterydetail.PrizePool = item.PoolRolling;
                lotterydetail.Sys_IssueId = item.Sys_IssueId;
                lotterydetail.LotteryResultDetail= JsonConvert.SerializeObject(item.openLotteryDetails);
                lotterydetail.CreateTime = DateTime.Now;

                count = db.Insertable(lotterydetail).ExecuteCommand();
                insertCount += count;
            }
            return await Task.FromResult(insertCount);
        }

        /// <summary>
        /// 根据彩种并且期号降序获取最新信息
        /// </summary>
        /// <returns></returns>
        public List<sys_issue> GetLotteryCodeList(string LotteryCode)
        {
            List<sys_issue> issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.IssueNo, OrderByType.Desc).ToList();
            if (issue == null)
            {
                return null;
            }
            else
            {
                return issue;
            }

        }

        /// <summary>
        /// 根据彩种并且开奖时间降序最新信息
        /// </summary>
        /// <returns></returns>
        public normal_lotterydetail GetNowIssuNo(string LotteryCode)
        {
            normal_lotterydetail issue = db.Queryable<normal_lotterydetail>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.OpenTime, OrderByType.Desc).Take(1).First();
            if (issue == null)
            {
                return null;
            }
            else
            {
                return issue;
            }

        }
        public sys_issue GetIssue(string IssueNo)
        {
            return db.Queryable<sys_issue>().Where(x => x.IssueNo == IssueNo).First();
        }
    
    }
}

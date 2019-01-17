using log4net;
using Lottery.Modes.Entity;
using Lottery.Modes.Model;
using Lottery.Services.Abstractions;
using Newtonsoft.Json;
using Smart.Core.Repository.SqlSugar;
using Smart.Core.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Smart.Core.Utils.CommonHelper;

namespace Lottery.Services
{
    public class LotteryDetailService : Repository<DbFactory>, ILotteryDetailService
    {
        //protected SqlSugarClient db = null;
        private ILog log;
        protected DbFactory BaseFactory;
        public LotteryDetailService(DbFactory factory) : base(factory)
        {
            BaseFactory = factory;
            log = LogManager.GetLogger("LotteryRepository", typeof(LotteryDetailService));
            //db = factory.GetDbContext();
        }

        public async Task<int> AddLotteryDetal(List<lotterydetail> lotterydetails,string gameCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                int count = 0;
                int insertCount = 0;
                var LotteryCode = db.Queryable<sys_lottery>().Where(x => x.LotteryCode == gameCode).First();
                //normal_lotterydetail nld = GetNowIssuNo(gameCode);
                foreach (var item in lotterydetails)
                {

                    //if (nld != null && gameCode != "sfc" && gameCode != "jq4" && gameCode != "zc6")
                    //{
                    //    if (nld.IssueNo == item.expect)
                    //    {
                    //        break;
                    //    }
                    //}
                    var strlotterydetail = GetCodelotterydetail(gameCode, item.expect);

                    if (strlotterydetail != null)
                    {
                            DateTime date = DateTime.Now.AddDays(-10);
                            if (NeedReGet(strlotterydetail.CurrentSales) && Convert.ToDateTime(strlotterydetail.OpenTime) > date)
                            {
                                strlotterydetail.LotteryDataDetail = item.teams.Count == 0 ? (item.NumberType != null ? item.openCode + "|" + item.NumberType : item.openCode) : JsonConvert.SerializeObject(item.teams);
                                strlotterydetail.LotteryResultDetail = item.dltLists.Count == 0 ? gameCode != "ttcx4" ? JsonConvert.SerializeObject(item.openLotteryDetails) : JsonConvert.SerializeObject(item.ttcx4Details) : JsonConvert.SerializeObject(item.dltLists);
                                strlotterydetail.UpdateTime = DateTime.Now;
                                strlotterydetail.PrizePool = item.PoolRolling;
                                strlotterydetail.CurrentSales = item.SalesVolume;
                                count = db.Updateable(strlotterydetail).ExecuteCommand();
                                log.Info(LotteryCode.LotteryName + "彩种" + strlotterydetail.IssueNo + "期期号更新完毕");
                            }
                        
                    }
                    else
                    {

                        normal_lotterydetail lotterydetail = new normal_lotterydetail();
                        lotterydetail.IssueNo = item.expect;
                        lotterydetail.LotteryId = LotteryCode.Lottery_Id;
                        lotterydetail.LotteryCode = LotteryCode.LotteryCode;
                        lotterydetail.OpenTime = item.openTime;
                        lotterydetail.AwardDeadlineTime = item.endTime;
                        lotterydetail.LotteryDataDetail = item.teams.Count == 0 ? (item.NumberType != null ? item.openCode + "|" + item.NumberType : item.openCode) : JsonConvert.SerializeObject(item.teams);
                        lotterydetail.CurrentSales = item.SalesVolume;
                        lotterydetail.PrizePool = item.PoolRolling;
                        lotterydetail.Sys_IssueId = item.Sys_IssueId;
                        lotterydetail.LotteryResultDetail = item.dltLists.Count == 0 ? gameCode != "ttcx4" ? JsonConvert.SerializeObject(item.openLotteryDetails) : JsonConvert.SerializeObject(item.ttcx4Details) : JsonConvert.SerializeObject(item.dltLists);
                        lotterydetail.CreateTime = DateTime.Now;
                        lotterydetail.Url_Type =(int)CollectionUrlEnum.url_500kaijiang;
                        count = db.Insertable(lotterydetail).ExecuteCommand();
                        log.Info(LotteryCode.LotteryName + "彩种" + item.expect + "期采集详情完毕");
                        insertCount += count;
                    }
                }
                return await Task.FromResult(insertCount);
            }
        }

        /// <summary>
        /// 根据彩种并且期号降序获取最新信息
        /// </summary>
        /// <returns></returns>
        public List<sys_issue> GetLotteryCodeList(string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                List<sys_issue> issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.OpenTime, OrderByType.Desc).ToList();
               
                return issue;
                
            }
        }

        /// <summary>
        /// 根据彩种并且开奖时间降序最新信息
        /// </summary>
        /// <returns></returns>
        public normal_lotterydetail GetNowIssuNo(string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                normal_lotterydetail issue = db.Queryable<normal_lotterydetail>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.OpenTime, OrderByType.Desc).First();
                
                return issue;
               
            }
        }


        public normal_lotterydetail GetCodelotterydetail(string LotteryCode, string IssueNo)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<normal_lotterydetail>().Where(n => n.LotteryCode == LotteryCode && n.IssueNo == IssueNo).OrderBy(n => n.IssueNo, OrderByType.Desc).First();
            }
        }
        public sys_issue GetIssue(string IssueNo)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(x => x.IssueNo == IssueNo).First();
            }
        }
    
    }
}

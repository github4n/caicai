using HtmlAgilityPack;
using log4net;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services
{
   public class JddDataService : Repository<DbFactory>, IJddDataService
    {
        private ILog log;
        protected DbFactory BaseFactory;
        public JddDataService(DbFactory factory) : base(factory)
        {
            BaseFactory = factory;
            log = LogManager.GetLogger("LotteryRepository", typeof(JddDataService));
            //db = factory.GetDbContext();
        }

        public  async Task<int> AddIssue(List<sys_issue> sys_Issues)
        {
          
                using (var db = BaseFactory.GetDbContext())
                {
                    int count = 0;
                    int insertCount = 0;
                    foreach (var item in sys_Issues)
                    {
                    
                        if (item.LotteryCode == "上海东方6＋1" || item.LotteryCode == "江苏15选5" || item.LotteryCode == "浙江20选5"
                            || item.LotteryCode == "浙江15选5" || item.LotteryCode == "安徽15选5" || item.LotteryCode == "福建15选5"
                            || item.LotteryCode == "江西15选5" || item.LotteryCode == "湖北30选5")
                        {
                            continue;
                        }
                        var Lottery = getlottery(item.LotteryCode);
                        if (Lottery == null)
                        {
                            Console.WriteLine(item.LotteryCode);
                        }
                        item.LotteryCode = Lottery.LotteryCode;
                        item.LotteryId = Lottery.Lottery_Id;
                        item.CreateTime = DateTime.Now;
                  
                        if (IsExistIssue(item.IssueNo, item.LotteryCode) == null)
                        {
                            count = db.Insertable(item).ExecuteCommand();
                            insertCount += count;
                            log.Info(Lottery.LotteryName+"彩种第"+ item.IssueNo+"期采集完毕");
                        }
                       
                    }
                    return await Task.FromResult(insertCount);
                }

        }

        public sys_lottery getlottery(string LotteryName)
        {

            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_lottery>().Where(n => n.LotteryName == ChangeLotteryName1(LotteryName)).First();

            }
        }

        public sys_issue IsExistIssue(string IssueNo,string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(n => n.IssueNo == IssueNo && n.LotteryCode== LotteryCode).First();
              
            }
        }

        public string ChangeLotteryName1(string LotteryName)
        {
            switch (LotteryName)
            {
                case "大乐透":
                    LotteryName = "超级大乐透";
                    break;
                case "排列三":
                    LotteryName = "排列3";
                    break;
                case "排列五":
                    LotteryName = "排列5";
                    break;
                case "胜负彩/任选九":
                    LotteryName = "足彩胜负(任选9场)";
                    break;
                case "6场半全场":
                    LotteryName = "6场半全";
                    break;
                case "4场进球彩":
                    LotteryName = "4场进球";
                    break;
                case "华东6省6+1":
                    LotteryName = "东方6+1";
                    break;
                case "上海天天彩选4":
                    LotteryName = "上海天天彩";
                    break;
                case "上海15选5":
                    LotteryName = "华东六省15选5";
                    break;
                case "江苏7位数":
                    LotteryName = "江苏七位数";
                    break;
                case "浙江6+1":
                    LotteryName = "浙江体彩6+1";
                    break;
                case "广东深圳风彩":
                    LotteryName = "深圳风彩";
                    break;
                case "华东6省15选5":
                    LotteryName = "华东六省15选5";
                    break;
                    
            }

            return LotteryName;
        }

    }
}

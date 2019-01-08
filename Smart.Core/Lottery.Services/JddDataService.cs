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
        private IXML_DataService xML_DataService;
        public JddDataService(DbFactory factory) : base(factory)
        {
            BaseFactory = factory;
            log = LogManager.GetLogger("LotteryRepository", typeof(LotteryDetailService));
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

                    var Lottery = getlottery(item.LotteryCode);
                    item.LotteryCode = Lottery.LotteryCode;
                    item.LotteryId = Lottery.Lottery_Id;
                    item.CreateTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    if (IsExistIssue(item.IssueNo) == null)
                    {
                        count = db.Insertable(item).ExecuteCommand();
                        insertCount += count;
                    }
                }
                return await Task.FromResult(insertCount);
            }
           
        }

        public sys_lottery getlottery(string LotteryName)
        {

            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_lottery>().Where(n => n.LotteryName == LotteryName).First();
            }
        }

        public sys_issue IsExistIssue(string IssueNo)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(n => n.IssueNo == IssueNo).First();
            }
        }
    }
}

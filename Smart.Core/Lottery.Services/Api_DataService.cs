using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Smart.Core.Throttle;
using Lottery.Modes.ShowModel;
using Smart.Core.NoSql.Redis;
using Smart.Core.Extensions;

namespace Lottery.Services
{
    public class Api_DataService: Repository<DbFactory>, IApi_DataService
    {
        protected SqlSugarClient db = null;
        public Api_DataService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }

        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <returns></returns>
        public List<sys_region> GetAreaList()
        {
            return db.Queryable<sys_region>().Where(p => p.IsShow == true).ToList();
        }

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        /// <returns></returns>
        public List<sys_lottery> GetAllLottery()
        {
            return db.Queryable<sys_lottery>().Where(p => p.IsShow == true).ToList();
        }

        /// <summary>
        /// 根据Code获取期号列表
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<sys_issue> GetLotteryIssuesByCode(string LotteryCode)
        {
            return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode).OrderBy(p=>p.OpenTime, OrderByType.Desc).Take(100).ToList();
        }

        /// <summary>
        /// 查询高频彩当前彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetHighLotteryIssues()
        {
            //弃用sql语句，速度太慢
//            SELECT* FROM
//(select i.*from sys_issue i inner
//           join sys_lottery l on (i.LotteryCode = l.LotteryCode)  where l.HighFrequency = 1
//ORDER BY i.OpenTime DESC limit 0,100000000)  t
//GROUP BY t.LotteryCode ORDER BY OpenTime DESC
          var highType = (int)HighFrequencyType.High;
            var key = "KJ_GetHighLotteryIssues";
            var str = RedisManager.DB_Other.Get(key);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
            }
            var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == highType).ToList();
            var result = new List<Issue_ShowModel>();
            if (lotteryList.Count > 0)
            {
                foreach (var item in lotteryList)
                {
                    var model= db.Queryable<sys_issue>().Where(p => p.LotteryCode == item.LotteryCode).OrderBy(p => p.OpenTime, OrderByType.Desc).First();
                    if (model != null)
                    {
                        result.Add(new Issue_ShowModel()
                        {
                            IssueNo = model.IssueNo,
                            LotteryCode = model.LotteryCode,
                            LotteryDay = item.LotteryDay,
                            LotteryId = item.Lottery_Id,
                            NumberPeriods = item.NumberPeriods,
                            OpenCode = model.OpenCode,
                            OpenTime = model.OpenTime,
                            RegionName = item.RegionName,
                            LotteryName = item.LotteryName
                        });
                    }
                }
            }
            if (result.Count > 0)
            {
                RedisManager.DB_Other.Set(key, result.ToJson(),60*30);
            }
            return result;
        }

        /// <summary>
        /// 查询地区彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetLocalLotteryIssues()
        {
            var key = "KJ_GetLocalLotteryIssues";
            var str = RedisManager.DB_Other.Get(key);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
            }
            var sql = @"SELECT i.IssueNo,i.LotteryCode,i.OpenCode,i.OpenTime,n.PrizePool
 FROM sys_issue i LEFT JOIN normal_lotterydetail n ON(i.IssueNo= n.IssueNo) WHERE i.lotteryCode = @LotteryCode ORDER BY i.OpenTime DESC LIMIT 0,1
";
            
            var localType = (int)HighFrequencyType.Local;
            var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == localType).ToList();
            var result = new List<Issue_ShowModel>();
            if (lotteryList.Count > 0)
            {
                foreach (var item in lotteryList)
                {
                    var model = db.SqlQueryable<Issue_ShowModel>(sql).AddParameters(new { LotteryCode = item.LotteryCode }).First();
                    if (model != null)
                    {
                        model.LotteryDay = item.LotteryDay;
                        model.LotteryId = item.Lottery_Id;
                        model.NumberPeriods = item.NumberPeriods;
                        model.RegionName = item.RegionName;
                        model.LotteryName = item.LotteryName;
                        result.Add(model);
                    }
                }
            }
            if (result.Count > 0)
            {
                RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 30);
            }
            return result;
        }

        /// <summary>
        /// 查询全国彩彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetCountryLotteryIssues()
        {
            var key = "KJ_GetCountryLotteryIssues";
            var str = RedisManager.DB_Other.Get(key);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
            }
            var sql = @"SELECT i.IssueNo,i.LotteryCode,i.OpenCode,i.OpenTime,n.PrizePool
 FROM sys_issue i LEFT JOIN normal_lotterydetail n ON(i.IssueNo= n.IssueNo) 
WHERE i.lotteryCode = @LotteryCode ORDER BY i.OpenTime DESC LIMIT 0,1";
            var localType = (int)HighFrequencyType.Country;
            var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == localType).ToList();
            var result = new List<Issue_ShowModel>();
            if (lotteryList.Count > 0)
            {
                foreach (var item in lotteryList)
                {
                    if (item.LotteryCode == "jczq" || item.LotteryCode == "jclq") continue;
                    var model = db.SqlQueryable<Issue_ShowModel>(sql).AddParameters(new { LotteryCode = item.LotteryCode }).First();
                    if (model != null)
                    {
                        model.LotteryDay = item.LotteryDay;
                        model.LotteryId = item.Lottery_Id;
                        model.NumberPeriods = item.NumberPeriods;
                        model.RegionName = item.RegionName;
                        model.LotteryName = item.LotteryName;
                        result.Add(model);
                    }
                }
            }
            if (result.Count > 0)
            {
                RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 30);
            }
            return result;
        }

        /// <summary>
        /// 根据时间和彩种拿高频彩数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="LotteryTime"></param>
        /// <returns></returns>
        public List<sys_issue> GetLotteryIssuesByCodeAndTime(string LotteryCode, DateTime LotteryTime)
        {
            var dt = LotteryTime.ToString("yyyy-MM-dd");
            return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode&&p.LotteryTime== dt).OrderBy(p => p.OpenTime, OrderByType.Desc).ToList();
        }

        /// <summary>
        /// 根据彩种号和时间获取期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="LotteryTime"></param>
        /// <returns></returns>
        //public List<sys_issue> GetLotteryIssuesDetail(string LotteryCode,string IssueNo)
        //{
        //    var sql="Select"
        //    if (string.IsNullOrEmpty(IssueNo))
        //    {

        //    }
        //    return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode && p.LotteryTime == dt).OrderBy(p => p.OpenTime, OrderByType.Desc).ToList();
        //}

    }
}

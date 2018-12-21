using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Smart.Core.Throttle;

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
            return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode).ToList();
        }

        /// <summary>
        /// 查询高频彩当前彩期列表
        /// </summary>
        /// <returns></returns>
        public List<sys_issue> GetHighFrequencyLotteryIssues()
        {
            var hightype = (int)HighFrequencyType.High;
            var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == hightype).ToList();

            if (lotteryList.Count > 0)
            {

            }
            return null;
        }
    }
}

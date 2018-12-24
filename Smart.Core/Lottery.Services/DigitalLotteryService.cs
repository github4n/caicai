using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using Newtonsoft.Json;

namespace Lottery.Services
{
    public class DigitalLotteryService : Repository<DbFactory>, IDigitalLotteryService
    {
        protected SqlSugarClient db = null;
        public DigitalLotteryService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }
        public void Addnormal_lotterydetail(List<fc3D> ModelList)
        {
            if (ModelList == null || ModelList.Count < 0)
            {
                return;
            }
            var LotteryCode = db.Queryable<sys_lottery>().Where(x => x.LotteryCode == "sd").First();
            if (LotteryCode == null)
            {
                return;
            }
            List<normal_lotterydetail> result = new List<normal_lotterydetail>();
            ModelList.ForEach((a) =>
            {
               bool b=db.Queryable<normal_lotterydetail>().Where(x => x.LotteryCode == LotteryCode.LotteryCode && x.IssueNo == a.expect).Count() > 0 ? true : false;
                if (!b)
                {
                    normal_lotterydetail _Lotterydetail = new normal_lotterydetail();
                    _Lotterydetail.LotteryId = LotteryCode.Lottery_Id;
                    _Lotterydetail.LotteryCode = LotteryCode.LotteryCode;
                    _Lotterydetail.IssueNo = a.expect;
                    _Lotterydetail.LotteryDataDetail = JsonConvert.SerializeObject(a.SubItemList);
                    _Lotterydetail.LotteryResultDetail = a.opencode + "|" + a.TestNumber + "|" + a.numberType;
                    _Lotterydetail.OpenTime = a.LotteryDate;
                    _Lotterydetail.AwardDeadlineTime = a.AwardDeadline;
                    _Lotterydetail.CurrentSales = a.SalesVolume;
                    db.Insertable(_Lotterydetail);
                }
            });
        } 
    }
}

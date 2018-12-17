using System;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using System.Threading.Tasks;
using Lottery.Modes.Entity;
using System.Collections.Generic;
using SqlSugar;

namespace Lottery.Services
{
    public class UsersService : Repository<DbFactory>, IUsersService
    {
        protected SqlSugarClient db = null;
        public UsersService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }


        public async Task<IList<blast_count>> TestMethod()
        {
            //获取数据库上下文
            //第一种  
            List<blast_count> list = new List<blast_count>();
            list =await db.Query<blast_count>();        
            blast_count model = new blast_count();
            model.typeid = 1000;
            model.playedId = 10000;
            model.createdate = DateTime.Now;
            model.betCount = 44;
            model.betAmount = 100;
            model.zjAmount = 100;
            //db.UseTransaction(()=> {
            //    db.Insertable<blast_count>(model).ExecuteCommand();
            //    test();
            //    return true;
            //});
            try
            {
                db.Ado.BeginTran();
                db.Insertable<blast_count>(model).ExecuteCommand();
                model.betCount = 4;
                db.Insertable<blast_count>(model).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                db.Ado.RollbackTran();
            }
            return list;
        }

        private void test()
        {
            blast_count model = new blast_count();
            model.typeid = 10001;
            model.playedId = 10000;
            model.createdate = DateTime.Now;
            model.betCount = 2;
            model.betAmount = 100;
            model.zjAmount = 100;
            db.Insertable<blast_count>(model).ExecuteCommand();
            //throw new Exception("ddd");
        }

    }
}

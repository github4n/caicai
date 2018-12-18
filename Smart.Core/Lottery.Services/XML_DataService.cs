using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Services
{
   public class XML_DataService: Repository<DbFactory>,IXML_DataService
    {
        protected SqlSugarClient db = null;
        public XML_DataService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }

        public void AddGdklsfAsync()
        {

            
        }
    }
}

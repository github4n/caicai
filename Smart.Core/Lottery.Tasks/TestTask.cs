using Lottery.Tasks.Abstractions;
using Smart.Core.Repository.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Tasks
{
    public class TestTask : Repository<DbFactory, IUsersRepository>, ITestTask
    {
        public TestTask()
        {

        }

        public Task<object> GetTestDb()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Tasks.Abstractions
{
    public interface ITestTask
    {
        Task<object> GetTestDb();
    }
}

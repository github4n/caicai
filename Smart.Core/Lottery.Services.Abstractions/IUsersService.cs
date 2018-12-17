using Lottery.Modes.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lottery.Services.Abstractions
{
    public interface IUsersService
    {
        Task<IList<blast_count>> TestMethod();
    }
}

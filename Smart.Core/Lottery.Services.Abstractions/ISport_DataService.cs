using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;

namespace Lottery.Services.Abstractions
{
   public interface ISport_DataService
    {
        void Add_BJDC(List<jczq> model);
        void Add_JCLQ();
        void Add_JCZQ(List<jczq> model);
    }
}

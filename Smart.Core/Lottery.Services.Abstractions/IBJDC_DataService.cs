using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;

namespace Lottery.Services.Abstractions
{
   public interface IBJDC_DataService
    {
        void AddBJDC(List<jczq> model);
    }
}

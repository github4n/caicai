using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;

namespace Lottery.Services.Abstractions
{
   public interface IDigitalLotteryService
    {
        void Addnormal_lotterydetail(List<fc3D> ModelList);
    }
}

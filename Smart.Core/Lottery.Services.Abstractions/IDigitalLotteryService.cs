using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;

namespace Lottery.Services.Abstractions
{
   public interface IDigitalLotteryService
    {
        void Addnormal_lotterydetail(List<fc3D> ModelList);
        void AddZqdc_Sfgg(List<zqdc_sfgg_result> zqdc_Sfggs);
        string Getnormal_lotteryIssue();
        string GetZqdc_Sfgg();
    }
}

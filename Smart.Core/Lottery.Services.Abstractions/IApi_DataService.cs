using Lottery.Modes.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Services.Abstractions
{
    public interface IApi_DataService 
    {
        /// <summary>
        /// 获取所有地区
        /// </summary>
        /// <returns></returns>
        List<sys_region> GetAreaList();

        /// <summary>
        /// 获取所有彩种
        /// </summary>
        /// <returns></returns>
        List<sys_lottery> GetAllLottery();
    }
}

using Lottery.Modes.Entity;
using Lottery.Modes.ShowModel;
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

        /// <summary>
        /// 根据彩种获取期号列表
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        List<sys_issue> GetLotteryIssuesByCode(string LotteryCode);

        /// <summary>
        /// 查询高频彩当前彩期列表
        /// </summary>
        /// <returns></returns>
        List<Issue_ShowModel> GetHighLotteryIssues();

        /// <summary>
        /// 地方彩彩期列表
        /// </summary>
        /// <returns></returns>
        List<Issue_ShowModel> GetLocalLotteryIssues();

        /// <summary>
        /// 查询全国彩彩期列表
        /// </summary>
        /// <returns></returns>
        List<Issue_ShowModel> GetCountryLotteryIssues();

        /// <summary>
        /// 根据时间和彩种拿高频彩数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="LotteryTime"></param>
        /// <returns></returns>
        List<sys_issue> GetLotteryIssuesByCodeAndTime(string LotteryCode, DateTime LotteryTime);
    }
}

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
        List<sys_issue> GetLotteryIssuesByCodeAndTime(string LotteryCode, string LotteryTime);

        /// <summary>
        /// 获取普通彩票详情（除足球篮球及高频彩外）
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IssueNo"></param>
        /// <returns></returns>
        NormalDetail_ShowModel GetLotteryDetail(string LotteryCode, string IssueNo);

        /// <summary>
        /// 根据时间获取竞彩足球详情
        /// </summary>
        /// <param name="LotteryDate"></param>
        /// <returns></returns>
        List<jczq_result> GetJCZQDetail(string LotteryDate);

        /// <summary>
        /// 根据时间获取竞彩篮球详情
        /// </summary>
        /// <param name="LotteryDate"></param>
        /// <returns></returns>
        List<jclq_result> GetJCLQDetail(string LotteryDate);

        /// <summary>
        /// 根据期号获取北京单场列表
        /// </summary>
        /// <param name="IssueNo"></param>
        /// <returns></returns>
        List<bjdc_result> GetZQDCDetail(string IssueNo);


        List<zqdc_sfgg_result> GetZQDCSFGGDetail(string IssueNo);

        void AddRedisHighLottery();
        void AddRedisLocalLottery();
        void AddRedisCountryLottery();
    }
}

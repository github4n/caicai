using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModel.Model;
using Lottery.Api.Controllers.CommonFilterActtribute;
using Lottery.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Extensions;
using Smart.Core.Filter;
using Smart.Core.JWT;
using Smart.Core.Throttle;

namespace Lottery.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ReusltFilter]
    public class DataController : ControllerBase
    {
        protected IApi_DataService _api_DataService;
        public DataController(IApi_DataService apiService)
        {
            this._api_DataService = apiService;
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public IActionResult GetNowDate()
        {
            return Ok(new LotteryServiceResponse()
            {
                Code = ResponseCode.成功,
                Value = DateTimeHelper.GetCurrentUnixTimeStamp()
            });
        }

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetArea()
        {
            try
            {
                var areaList = _api_DataService.GetAreaList();
                return Ok(
                    new LotteryServiceResponse()
                    {
                        Code = ResponseCode.成功,
                        Value = areaList,
                        Message = "获取成功"
                    });
            }
            catch (Exception ex)
            {
                return Ok(
                       new LotteryServiceResponse()
                       {
                           Code = ResponseCode.失败,
                           Message = "获取失败",
                           Value = ex.Message
                       });
            }
        }

        /// <summary>
        /// 获取所有彩种
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetAllLottery()
        {
            try
            {
                var lotteryList = _api_DataService.GetAllLottery();
                return Ok(
                    new LotteryServiceResponse()
                    {
                        Code = ResponseCode.成功,
                        Value = lotteryList,
                        Message = "获取成功"
                    });
            }
            catch (Exception ex)
            {
                return Ok(
                       new LotteryServiceResponse()
                       {
                           Code = ResponseCode.失败,
                           Message = "获取失败",
                           Value = ex.Message
                       });
            }
        }

        /// <summary>
        /// 根据彩种Code获取期号
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetLotteryIssuesByCode([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string LotteryCode = p.LotteryCode;
                if (string.IsNullOrEmpty(LotteryCode))
                {
                    return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.失败,
                            Message = "获取失败"
                        });
                }
                var issuesList = _api_DataService.GetLotteryIssuesByCode(LotteryCode);
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.成功,
                            Value = issuesList.Select(c=>new {
                                IssueNo=c.IssueNo,
                                OpenTime=c.OpenTime,
                                OpenCode=c.OpenCode
                            }),
                            Message = "获取成功"
                        });
            }
            catch (Exception ex)
            {
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.失败,
                            Message = "获取失败",
                             Value=ex.Message
                        });
            }
        }

        /// <summary>
        /// 获取高频彩首页的接口
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetHighLotteryIssues()
        {
            try
            {
                var issuesList = _api_DataService.GetHighLotteryIssues();
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.成功,
                            Value = issuesList,
                            Message = "获取成功"
                        });
            }
            catch (Exception ex)
            {
                return Ok(
                       new LotteryServiceResponse()
                       {
                           Code = ResponseCode.失败,
                           Message = "获取失败",
                           Value = ex.Message
                       });
            }
        }

        /// <summary>
        /// 获取地方彩种的首页接口
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetLocalLotteryIssues()
        {
            try
            {
                var issuesList = _api_DataService.GetLocalLotteryIssues();
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.成功,
                            Value = issuesList,
                            Message = "获取成功"
                        });
            }
            catch (Exception ex)
            {
                return Ok(
                       new LotteryServiceResponse()
                       {
                           Code = ResponseCode.失败,
                           Message = "获取失败",
                           Value = ex.Message
                       });
            }
        }

        /// <summary>
        /// 获取全国发售彩种的首页接口
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetCountryLotteryIssues()
        {
            try
            {
                var issuesList = _api_DataService.GetCountryLotteryIssues();
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.成功,
                            Value = issuesList,
                            Message = "获取成功"
                        });
            }
            catch (Exception ex)
            {
                return Ok(
                       new LotteryServiceResponse()
                       {
                           Code = ResponseCode.失败,
                           Message = "获取失败",
                           Value = ex.Message
                       });
            }
        }

        //public IActionResult GetLotteryIssuesByCodeAndTime()
    }
}
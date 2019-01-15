using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityModel.Model;
using Lottery.Api.Controllers.CommonFilterActtribute;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Core.Extensions;
using Smart.Core.Filter;
using Smart.Core.JWT;
using Smart.Core.Throttle;
using Smart.Core.Extensions;

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
        [HttpPost]
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
                lotteryList.ForEach(p => p.WebLogo += p.LotteryCode + ".jpg");
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
        /// <param name="entity"></param>
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
                if (LotteryCode.ToLower() == "zqdc" || LotteryCode.ToLower() == "zqdcsfgg")
                {
                    issuesList = issuesList.OrderByDescending(c => c.IssueNo).ToList();
                }
                return Ok(
                        new LotteryServiceResponse()
                        {
                            Code = ResponseCode.成功,
                            Value = issuesList.Select(c => new
                            {
                                IssueNo = c.IssueNo,
                                OpenTime = c.OpenTime.FormatDate(),
                                OpenCode = c.OpenCode
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
                            Value = ex.Message
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

        /// <summary>
        /// 根据时间和彩种拿高频彩列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetLotteryIssuesByCodeAndTime([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string LotteryCode = p.LotteryCode;
                string LotteryTime = p.LotteryTime;
                var list = _api_DataService.GetLotteryIssuesByCodeAndTime(LotteryCode, LotteryTime);
                return Ok(
                               new LotteryServiceResponse()
                               {
                                   Code = ResponseCode.成功,
                                   Value = list,
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
        /// 获取普通彩票详情（除足球篮球及高频彩外）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetLotteryDetail([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string LotteryCode = p.LotteryCode;
                string IssueNo = p.IssueNo;
                var list = _api_DataService.GetLotteryDetail(LotteryCode, IssueNo);
                return Ok(
                           new LotteryServiceResponse()
                           {
                               Code = ResponseCode.成功,
                               Value = list,
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
        /// 获取竞彩足球详情列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetJCZQList([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string LotteryDate = p.LotteryDate;
                var list = _api_DataService.GetJCZQDetail(LotteryDate);
                return Ok(
                           new LotteryServiceResponse()
                           {
                               Code = ResponseCode.成功,
                               Value = list,
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
        /// 获取竞彩足球详情列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetJCLQList([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string LotteryDate = p.LotteryDate;
                var list = _api_DataService.GetJCLQDetail(LotteryDate);
                return Ok(
                           new LotteryServiceResponse()
                           {
                               Code = ResponseCode.成功,
                               Value = list,
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
        /// 足球单场列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult GetZQDCDetail([FromForm] LotteryServiceRequest entity)
        {
            try
            {
                var p = JsonHelper.Decode(entity.Param);
                string ZQDCType = p.ZQDCType;
                string IssueNo = p.IssueNo;
                //var result = new List<bjdc_result>();
                if (ZQDCType == "0")
                {
                    var result = _api_DataService.GetZQDCDetail(IssueNo);
                    return Ok(
                          new LotteryServiceResponse()
                          {
                              Code = ResponseCode.成功,
                              Value = result,
                              Message = "获取成功"
                          });
                }
                else
                {
                    var result = _api_DataService.GetZQDCSFGGDetail(IssueNo);
                    return Ok(
                          new LotteryServiceResponse()
                          {
                              Code = ResponseCode.成功,
                              Value = result,
                              Message = "获取成功"
                          });
                }
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
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Throttle
{
    /// <summary>
    /// 节流策略
    /// </summary>
    /// <remarks>修改时请务必同步修改HttpContextExtension=>GetPolicyValue方法</remarks>
    public enum Policy : short
    {
        /// <summary>
        /// IP地址
        /// </summary>
        Ip = 1,

        /// <summary>
        /// 用户身份
        /// </summary>
        UserIdentity = 2,

        /// <summary>
        /// Request Header
        /// </summary>
        Header = 3,

        /// <summary>
        /// Request Query
        /// </summary>
        Query = 4,

        /// <summary>
        /// 网址 Request path
        /// </summary>
        RequestPath = 5,

        /// <summary>
        /// Cookie
        /// </summary>
        Cookie = 6,

        /// <summary>
        /// Request Form
        /// </summary>
        Form = 7
    }

    /// <summary>
    /// 当识别值为空时处理方式
    /// </summary>
    public enum WhenNull : short
    {
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 0,

        /// <summary>
        /// 拦截
        /// </summary>
        Intercept = 1
    }

    /// <summary>
    /// 拦截位置
    /// </summary>
    public enum IntercepteWhere
    {
        ActionFilter,

        PageFilter,

        Middleware
    }

    public enum RosterType : short
    {
        /// <summary>
        /// 黑名单
        /// </summary>
        BlackList = 1,

        /// <summary>
        /// 白名单
        /// </summary>
        WhiteList = 2
    }

    public enum ResponseCode
    {
        成功 = 101,
        失败 = 201,
        //验证码错误
        ValiteCodeError = 301,
        TimeStampError = 401,
    }

    public enum SchemeSource
    {
        Web = 0,
        Iphone = 101,
        Android = 102,
        Wap = 103,
        Touch = 104,
        YQS = 105,
        YQS_Advertising = 106,
        NS_Bet = 107,
        YQS_Bet = 108,
        Publisher_0321 = 109,
        WX_GiveLottery = 110,
        Web_GiveLottery = 111,
        LuckyDraw = 112,
        NewIphone = 113,
        NewAndroid = 115,
        NewWeb = 116
    }

    public enum HighFrequencyType
    {
        /// <summary>
        /// 全国彩
        /// </summary>
        Country=0,
        /// <summary>
        /// 高频彩
        /// </summary>
        High=1,
        /// <summary>
        /// 地方彩
        /// </summary>
        Local=2
    }
}

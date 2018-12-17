using Smart.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.GatherApp.Helper
{
    public class UseFullHelper
    {
        public static string SafeCode = "newxingcai";

        public static string GetWjorderId(string gamecoe, int typeid, int playid)
        {
            return $"{gamecoe}{typeid}{playid}{CreateRechargeId()}";
        }

        /// <summary>
        /// 生成充值订单号
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRechargeId(int length = 5)
        {
            return $"{DateTime.Now.LocalDateTimeToJavaTimeStamp()}{GetUUID(length)}";
        }

        public static string UUID(int length = 10)
        {
            return DateTime.Now.ToString("MMddHHmmssmmm") + GetUUID(length);
        }

        public static string GetUUID(int length)
        {
            var buffer = Guid.NewGuid().ToByteArray();
            var code = System.Math.Abs(BitConverter.ToInt32(buffer, 0)).ToString();
            if (code.Length >= length)
                return code.Substring(0, length);

            var diffLength = length - code.Length;
            var diffCode = GetUUID(diffLength);
            return string.Format("{0}{1}", diffCode, code);
        }

        /// <summary>
        /// 用户名和用户id
        /// </summary>
        /// <param name="IdentityId"></param>
        /// <returns></returns>
        public static (int, string) UserName(string IdentityId)
        {
            var arr = IdentityId.Split('|');
            int uid = Convert.ToInt32(arr[0]);
            string username = arr[1];
            return (uid, username);
        }

        /// <summary>
        /// 格式化期号
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="now"></param>
        /// <param name="actionNo"></param>
        /// <param name="bagintime"></param>
        /// <returns></returns>
        public static string NumberFormat(int type, string date, DateTime now, int actionNo, ulong bagintime)
        {
            string number = string.Empty;
            if (type == 1)//重庆时时彩
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#000"));
            }
            else if (type == 12)//新疆时时彩
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#00"));
            }
            else if (type == 60) //天津时时彩
            {
                //number = string.Format("{0}{1}", now.ToString("yyMMdd"), actionNo.ToString("#000"));
                number = string.Format("{0}-{1}", date, actionNo.ToString("#000"));
            }
            else if (type == 61 || type == 62)//澳门时时彩 台湾时时彩
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#000"));
            }
            else if (type == 7 || type == 15 || type == 6 || type == 16 || type == 68 || type == 67)//7山东11选5  6广东11X5  15上海11选5  16江西11选5  68台湾11X5 67  澳门11X5
            {
                number = string.Format("{0}{1}", now.ToString("yyyyMMdd"), actionNo.ToString("#00"));
            }


            else if (type == 5)// 分分彩特殊处理
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#0000"));
            }

            else if (type == 75 || type == 77 || type == 76)//澳门时时彩 台湾时时彩 巴西快乐彩   巴西1.5分彩
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#000"));
            }
            else if (type == 16 || type == 15)//江西11选5 山东11选5 上海11选5
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#00"));
            }


            else if (type == 9 || type == 10)// 福彩3D 排列3 获取天数
            {
                number = string.Format("{0}{1}", now.Year, now.DayOfYear - 7);
            }
            else if (type == 79)//江苏快3  20171020048
            {
                number = string.Format("{0}{1}", date, actionNo.ToString("#000"));
            }
            else if (type == 71 || type == 72)//幸运农场
            {
                number = string.Format("{0}{1}", now.ToString("yyyyMMdd"), actionNo.ToString("#000"));
            }
            else if (type == 11)// 时时乐
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#00"));
            }
            else if (type == 20)////北京PK10
            {
                number = (179 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2007-11-11"))) / 3600 / 24 + (ulong)actionNo - 3793).ToString();
            }
            else if (type == 65)//澳门PK10
            {
                number = (288 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2007-11-11"))) / 3600 / 24 + (ulong)actionNo - 6789).ToString();
            }
            else if (type == 66)//台湾PK10
            {
                number = (288 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2007-11-11"))) / 3600 / 24 + (ulong)actionNo - 4321).ToString();
            }
            else if (type == 78)//北京快乐8
            {
                number = (179 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2004-09-19"))) / 3600 / 24 + (ulong)actionNo - 3857).ToString();
            }
            else if (type == 73)//澳门快乐8
            {
                number = (288 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2004-09-19"))) / 3600 / 24 + (ulong)actionNo - 1234).ToString();
            }
            else if (type == 74)// 韩国快乐8
            {
                number = (288 * (bagintime - DateTimeHelper.LocalDateTimeToUnixTimeStamp(Convert.ToDateTime("2004-09-19"))) / 3600 / 24 + (ulong)actionNo - 4567).ToString();
            }
            else if (type == 34)
            {
                number = $"{now.Year}-{actionNo.ToString("#000")}";
            }
            else
            {
                number = string.Format("{0}-{1}", date, actionNo.ToString("#000"));
            }
            return number;
        }



        /// <summary>
        /// 格式化期号
        /// </summary>
        public static string FormatIssuseNumber(string gameCode, string oldIssuseNumber)
        {
            var issuseNumber = oldIssuseNumber;
            switch (gameCode.ToUpper())
            {
                case "CQSSC":
                case "TJSSC":
                case "XJSSC":
                    issuseNumber = issuseNumber.Insert(8, "-");
                    break;

                case "JX11X5":
                case "GD11X5":
                    break;
                case "SD11X5":
                    issuseNumber = issuseNumber.Replace("-", "");
                    break;
                case "SDKLPK3":
                    issuseNumber = issuseNumber.Insert(8, "-");
                    break;
                case "JSKS":
                case "GDKLSF":
                    issuseNumber = issuseNumber.Insert(8, "-");
                    var t = issuseNumber.Split('-');
                    if (t.Length == 2)
                    {
                        issuseNumber = t[0] + "-" + t[1].Substring(1);
                    }
                    break;
                case "PL3":
                    if (issuseNumber.Length == 5)
                    {
                        issuseNumber = $"{DateTime.Now.ToString("yyyy").Substring(0, 2)}{issuseNumber}";
                    }
                    break;
                case "JSK3":
                    if (issuseNumber.Length == 9)
                    {
                        issuseNumber = $"{DateTime.Now.ToString("yyyy").Substring(0, 2)}{issuseNumber}";
                    }
                    break;
                case "SSQ":
                case "DLT":
                    issuseNumber = issuseNumber.Insert(4, "-");
                    break;

                default:
                    break;
            }
            return issuseNumber;
        }



        /// <summary>
        /// 格式化开奖号码
        /// </summary>
        public static string FormatWinNumber(string gameCode, string oldCode)
        {
            var winNumber = oldCode;
            switch (gameCode.ToUpper())
            {
                case "SH11X5":
                    if (!oldCode.Contains(","))
                    {
                        winNumber = string.Join(",", winNumber.Split(' '));
                    }
                    break;
                case "CQSSC":
                case "JX11X5":
                case "SD11X5":
                case "GD11X5":
                case "GDKLSF":
                case "JSKS":
                case "SDKLPK3":
                case "FC3D":
                case "PL3":
                    break;
                case "SSQ":
                case "DLT":
                    winNumber = winNumber.Replace("+", "|");
                    break;

                default:
                    break;
            }
            return winNumber;
        }


        /// <summary>
        /// 判断是否是非法字符
        /// </summary>
        /// <param name="str">判断是字符</param>
        /// <returns></returns>
        public static Boolean isLegalNumber(string str)
        {
            char[] charStr = str.ToLower().ToCharArray();
            for (int i = 0; i < charStr.Length; i++)
            {
                int num = Convert.ToInt32(charStr[i]);
                if (!(IsChineseLetter(num) || (num >= 48 && num <= 57) || (num >= 97 && num <= 123) || (num >= 65 && num <= 90) || num == 45))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsNumber(string str)
        {
            char[] charStr = str.ToLower().ToCharArray();
            for (int i = 0; i < charStr.Length; i++)
            {
                int num = Convert.ToInt32(charStr[i]);
                if (!(num >= 48 && num <= 57))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 判断字符的Unicode值是否是汉字
        /// </summary>
        /// <param name="code">字符的Unicode</param>
        /// <returns></returns>
        protected static bool IsChineseLetter(int code)
        {
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);

            if (code >= chfrom && code <= chend)
            {
                return true;     //当code在中文范围内返回true

            }
            else
            {
                return false;    //当code不在中文范围内返回false
            }
        }
    }
}

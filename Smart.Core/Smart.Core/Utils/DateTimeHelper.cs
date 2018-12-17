using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Smart.Core.Utils
{
    public partial class DateTimeHelper
    {
        public static readonly DateTime UTCOrigin;
        static DateTimeHelper()
        {
            UTCOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }


        public static string GetToday()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string GetToday(string format)
        {
            return DateTime.Now.ToString(format);
        }

        public static string GetDate(int i)
        {
            return DateTime.Now.AddDays((double)i).ToString("yyyy-MM-dd");
        }

        public static string GetNumberWeekDay(DateTime dt)
        {
            int y = dt.Year;
            int i = dt.Month;
            int d = dt.Day;
            if (i < 3)
            {
                i += 12;
                y--;
            }
            if (y % 400 == 0 || (y % 100 != 0 && y % 4 == 0))
            {
                d--;
            }
            else
            {
                d++;
            }
            return ((d + 2 * i + 3 * (i + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7).ToString();
        }

        public string GetChineseWeekDay(int y, int m, int d)
        {
            string[] weekstr = new string[]
			{
				"日",
				"一",
				"二",
				"三",
				"四",
				"五",
				"六"
			};
            if (m < 3)
            {
                m += 12;
                y--;
            }
            if (y % 400 == 0 || (y % 100 != 0 && y % 4 == 0))
            {
                d--;
            }
            else
            {
                d++;
            }
            return "星期" + weekstr[(d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7];
        }

        public static int GetDaysOfYear(int iYear)
        {
            return DateTimeHelper.IsRuYear(iYear) ? 366 : 365;
        }

        public static int GetDaysOfYear(DateTime dt)
        {
            return DateTimeHelper.IsRuYear(dt.Year) ? 366 : 365;
        }

        public static int GetDaysOfMonth(int iYear, int Month)
        {
            int days = 0;
            switch (Month)
            {
                case 1:
                    days = 31;
                    break;

                case 2:
                    days = (DateTimeHelper.IsRuYear(iYear) ? 29 : 28);
                    break;

                case 3:
                    days = 31;
                    break;

                case 4:
                    days = 30;
                    break;

                case 5:
                    days = 31;
                    break;

                case 6:
                    days = 30;
                    break;

                case 7:
                    days = 31;
                    break;

                case 8:
                    days = 31;
                    break;

                case 9:
                    days = 30;
                    break;

                case 10:
                    days = 31;
                    break;

                case 11:
                    days = 30;
                    break;

                case 12:
                    days = 31;
                    break;
            }
            return days;
        }

        public static int GetDaysOfMonth(DateTime dt)
        {
            int days = 0;
            int year = dt.Year;
            switch (dt.Month)
            {
                case 1:
                    days = 31;
                    break;

                case 2:
                    days = (DateTimeHelper.IsRuYear(year) ? 29 : 28);
                    break;

                case 3:
                    days = 31;
                    break;

                case 4:
                    days = 30;
                    break;

                case 5:
                    days = 31;
                    break;

                case 6:
                    days = 30;
                    break;

                case 7:
                    days = 31;
                    break;

                case 8:
                    days = 31;
                    break;

                case 9:
                    days = 30;
                    break;

                case 10:
                    days = 31;
                    break;

                case 11:
                    days = 30;
                    break;

                case 12:
                    days = 31;
                    break;
            }
            return days;
        }

        public static string GetWeekNameOfDay(DateTime dt)
        {
            string week = string.Empty;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = "星期日";
                    break;

                case DayOfWeek.Monday:
                    week = "星期一";
                    break;

                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;

                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;

                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;

                case DayOfWeek.Friday:
                    week = "星期五";
                    break;

                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
            }
            return week;
        }

        public static int GetWeekNumberOfDay(DateTime dt)
        {
            int week = 0;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = 7;
                    break;

                case DayOfWeek.Monday:
                    week = 1;
                    break;

                case DayOfWeek.Tuesday:
                    week = 2;
                    break;

                case DayOfWeek.Wednesday:
                    week = 3;
                    break;

                case DayOfWeek.Thursday:
                    week = 4;
                    break;

                case DayOfWeek.Friday:
                    week = 5;
                    break;

                case DayOfWeek.Saturday:
                    week = 6;
                    break;
            }
            return week;
        }

        public static int GetWeekAmount(int year)
        {
            DateTime end = new DateTime(year, 12, 31);
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static void WeekRange(int year, int weekOrder, ref DateTime firstDate, ref DateTime lastDate)
        {
            DateTime firstDay = new DateTime(year, 1, 1);
            int firstOfWeek = Convert.ToInt32(firstDay.DayOfWeek);
            int dayDiff = -1 * firstOfWeek + 1;
            int dayAdd = 7 - firstOfWeek;
            firstDate = firstDay.AddDays((double)dayDiff).Date;
            lastDate = firstDay.AddDays((double)dayAdd).Date;
            if (weekOrder != 1)
            {
                int addDays = (weekOrder - 1) * 7;
                firstDate = firstDate.AddDays((double)addDays);
                lastDate = lastDate.AddDays((double)addDays);
            }
        }

        public static int DiffDays(DateTime dtfrm, DateTime dtto)
        {
            return (dtto.Date - dtfrm.Date).Days;
        }

        private static bool IsRuYear(int iYear)
        {
            return iYear % 400 == 0 || (iYear % 4 == 0 && iYear % 100 != 0);
        }

        public static DateTime ToDate(string strInput)
        {
            DateTime oDateTime;
            try
            {
                oDateTime = DateTime.Parse(strInput);
            }
            catch (Exception)
            {
                oDateTime = DateTime.Today;
            }
            return oDateTime;
        }

        public static string ToString(DateTime oDateTime, string strFormat)
        {
            string strDate;
            try
            {
                string text = strFormat.ToUpper();
                if (text != null)
                {
                    if (text == "SHORTDATE")
                    {
                        strDate = oDateTime.ToShortDateString();
                        goto IL_47;
                    }
                    if (text == "LONGDATE")
                    {
                        strDate = oDateTime.ToLongDateString();
                        goto IL_47;
                    }
                }
                strDate = oDateTime.ToString(strFormat);
            IL_47: ;
            }
            catch (Exception)
            {
                strDate = oDateTime.ToShortDateString();
            }
            return strDate;
        }


        //==============================================================================================
        /// <summary>
        /// 取每月的第一/最末一天
        /// </summary>
        /// <param name="time">传入时间</param>
        /// <param name="firstDay">第一天还是最末一天</param>
        /// <returns></returns>
        public static DateTime DayOfMonth(DateTime time, bool firstDay)
        {
            DateTime time1 = new DateTime(time.Year, time.Month, 1);
            if (firstDay) return time1;
            else return time1.AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 取每季度的第一/最末一天
        /// </summary>
        /// <param name="time">传入时间</param>
        /// <param name="firstDay">第一天还是最末一天</param>
        /// <returns></returns>
        public static DateTime DayOfQuarter(DateTime time, bool firstDay)
        {
            int m = 0;
            switch (time.Month)
            {
                case 1:
                case 2:
                case 3:
                    m = 1; break;
                case 4:
                case 5:
                case 6:
                    m = 4; break;
                case 7:
                case 8:
                case 9:
                    m = 7; break;
                case 10:
                case 11:
                case 12:
                    m = 11; break;
            }

            DateTime time1 = new DateTime(time.Year, m, 1);
            if (firstDay) return time1;
            else return time1.AddMonths(3).AddDays(-1);
        }
        /// <summary>
        /// 取每年的第一/最末一天
        /// </summary>
        /// <param name="time">传入时间</param>
        /// <param name="firstDay">第一天还是最末一天</param>
        /// <returns></returns>
        public static DateTime DayOfYear(DateTime time, bool firstDay)
        {
            if (firstDay) return new DateTime(time.Year, 1, 1);
            else return new DateTime(time.Year, 12, 31);
        }



        /// <summary>
        /// 返回标准日期格式string(yyyy-MM-dd)
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }


        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回标准时间 
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {

                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        //========================================================================================================
        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }

        /// <summary>
        /// 取时间戳生成随即数,替换交易单号中的后10位流水号
        /// </summary>
        /// <returns></returns>
        public static int UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt32(ts.TotalSeconds);
        }


        /// <summary>
        /// 取时间戳
        /// </summary>
        /// <returns></returns>
        public static int UnixStamp(DateTime now)
        {
            TimeSpan ts = now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt32(ts.TotalSeconds);
        }


        public static ulong LocalDateTimeToUnixTimeStamp(DateTime date)
        {
            return (ulong)Math.Floor((date.ToUniversalTime() - UTCOrigin).TotalSeconds);
        }
    }
}
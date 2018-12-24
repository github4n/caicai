using System;

namespace Smart.Core.Extensions
{
    public static class DateTimeHelper
    {
        public static readonly DateTime UTCOrigin;

        static DateTimeHelper()
        {
            UTCOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime GetLocalTime(this DateTime universal)
        {
            return universal.ToLocalTime();
        }

        public static DateTime GetUniversalTime(this DateTime local)
        {
            return local.ToUniversalTime();
        }

        public static string GetDateString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        public static string GetTimeString(this DateTime datetime)
        {
            return datetime.ToString("HH:mm:ss");
        }

        public static string GetDateTimeString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateTimeWithMillisecondsString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static ulong LocalDateTimeToUnixTimeStamp(this DateTime date)
        {
            return (ulong)Math.Floor((date.ToUniversalTime() - UTCOrigin).TotalSeconds);
        }

        public static DateTime UnixTimeStampToLocalDateTime(this ulong timestamp)
        {
            return UTCOrigin.ToLocalTime().AddSeconds(timestamp);
        }

        public static ulong UniversalDateTimeToUnixTimeStamp(this DateTime date)
        {
            return (ulong)Math.Floor((date - UTCOrigin).TotalSeconds);
        }

        public static DateTime UnixTimeStampToUniversalDateTime(this ulong timestamp)
        {
            return UTCOrigin.AddSeconds(timestamp);
        }

        public static ulong GetCurrentUnixTimeStamp()
        {
            return LocalDateTimeToUnixTimeStamp(DateTime.Now);
        }

        public static ulong LocalDateTimeToJavaTimeStamp(this DateTime date)
        {
            return (ulong)Math.Floor((date.ToUniversalTime() - UTCOrigin).TotalMilliseconds);
        }

        public static DateTime JavaTimeStampToLocalDateTime(this ulong timestamp)
        {
            return UTCOrigin.ToLocalTime().AddMilliseconds(timestamp);
        }

        public static ulong UniversalDateTimeToJavaTimeStamp(this DateTime date)
        {
            return (ulong)Math.Floor((date - UTCOrigin).TotalMilliseconds);
        }

        public static DateTime JavaTimeStampToUniversalDateTime(this ulong timestamp)
        {
            return UTCOrigin.AddMilliseconds(timestamp);
        }

        public static ulong GetCurrentJavaTimestamp()
        {
            return LocalDateTimeToJavaTimeStamp(DateTime.Now);
        }

        public static DateTime StampToDateTime(string timeStamp)
        {
            var t = Convert.ToUInt32(timeStamp);
            return UTCOrigin.AddSeconds(t).AddHours(8);
        }
    }
}

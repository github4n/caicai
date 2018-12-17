using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Smart.Core.Logger
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 所有日志，记录DEBUG|INFO|WARN|ERROR|FATAL级别的日志
        /// </summary>
        DEBUG,

        /// <summary>
        /// 记录INFO|WARN|ERROR|FATAL级别的日志
        /// </summary>
        INFO,

        /// <summary>
        /// 记录WARN|ERROR|FATAL级别的日志
        /// </summary>
        WARN,

        /// <summary>
        /// 记录ERROR|FATAL级别的日志
        /// </summary>
        ERROR,

        /// <summary>
        /// 记录FATAL级别的日志
        /// </summary>
        FATAL,

        /// <summary>
        /// 关闭日志功能
        /// </summary>
        OFF,
    }

    /// <summary>
    /// 日志核心基类
    /// 模版方法模式，对InputLogger开放，对其它日志逻辑隐藏，InputLogger可以有多种实现
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        #region Protected Fields

        /// <summary>
        /// 日志文件地址
        /// 优化级为mvc方案地址，网站方案地址，console程序地址
        /// </summary>
        [ThreadStatic]
        static protected string FileUrl = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        /// <summary>
        /// 日志文件名
        /// </summary>
        protected string _defaultLoggerName = DateTime.Now.ToString("yyyyMMdd");

        #endregion Protected Fields

        #region Private Fields

        /// <summary>
        /// 日志项目类型
        /// </summary>
        protected string projectName = "日志聚集";

        #endregion Private Fields

        #region Protected Methods

        /// <summary>
        /// 格式化字符
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string FormatStr(string level, string message, Exception ex)
        {
            #region 一个请求的traceId是唯一的

            string val;
            Microsoft.Extensions.Primitives.StringValues traceId;
            try
            {
                Microsoft.Extensions.DependencyInjection.HttpContext.Current.Request.Headers.TryGetValue("traceId", out traceId);
                if (traceId.Count > 0)
                {
                    val = traceId.FirstOrDefault().ToString();
                }
                else
                {
                    val = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(3, '0');
                    Microsoft.Extensions.DependencyInjection.HttpContext.Current.Request.Headers.Add("traceId", val);
                }
            }
            catch (Exception)
            {
                val = string.Empty;
            }

            #endregion 一个请求的traceId是唯一的

            var json = JsonConvert.SerializeObject(new
            {
                traceId = val,
                target_index = projectName,
                timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800"),
                Level = level.ToString(),
                Message = message,
                StackTrace = ex?.StackTrace
            });
            json = json.Replace("target_index", "@target_index").Replace("timestamp", "@timestamp");
            return json;
        }

        /// <summary>
        /// 日志持久化的方法，派生类必须要实现自己的方式
        /// </summary>
        /// <param name="message"></param>
        protected abstract void InputLogger(LogLevel level, string message);

        #endregion Protected Methods

        #region ILogger 成员

        /// <summary>
        /// 占位符
        /// </summary>
        private const int LEFTTAG = 7;

        public virtual void Debug(string message)
        {
            InputLogger(LogLevel.DEBUG, message);
            Trace.WriteLine(message);
        }

        public virtual void Error(string message, Exception ex)
        {
            InputLogger(LogLevel.ERROR, message + ex.ToString());
            Trace.WriteLine(message + ex.ToString());
        }

        public virtual void Fatal(string message)
        {
            InputLogger(LogLevel.FATAL, message);
            Trace.WriteLine(message);
        }

        public virtual void Info(string message)
        {
            InputLogger(LogLevel.INFO, message);
            Trace.WriteLine(message);
        }

        public virtual void Warn(string message)
        {
            InputLogger(LogLevel.FATAL, message);
            Trace.WriteLine(message);
        }

        #endregion ILogger 成员
    }
}

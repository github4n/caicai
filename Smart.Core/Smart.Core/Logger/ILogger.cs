using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.Logger
{
    /// <summary>
    /// 日志功能接口规范
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 调试期间的日志
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// 异常发生的日志
        /// </summary>
        /// <param name="message"></param>
        void Error(string message, Exception ex);

        /// <summary>
        /// 引起程序终止的日志
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message);

        /// <summary>
        /// 将message记录到日志文件
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 引起警告的日志
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);
    }
}

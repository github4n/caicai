using System;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Smart.Core.Logger
{
    /// <summary>
    /// 以普通的文字流的方式写日志
    /// </summary>
    public class TextFileLogger : LoggerBase
    {
        #region Private Fields

        private static readonly object objLock = new object();

        #endregion Private Fields

        #region Protected Methods

        /// <summary>
        /// 实现基类－写日志文件的逻辑
        /// </summary>
        /// <param name="message"></param>
        protected override void InputLogger(LogLevel level, string message)
        {
            message = FormatStr(level.ToString(), message, null);

            if (string.IsNullOrWhiteSpace(FileUrl))
                FileUrl = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!System.IO.Directory.Exists(FileUrl))
                System.IO.Directory.CreateDirectory(FileUrl);
            string filePath = Path.Combine(FileUrl, _defaultLoggerName);

            //写日志委托
            Action<string> write = (fileName) =>
            {
                lock (objLock)//防治多线程读写冲突
                {
                    using (var file = new System.IO.StreamWriter(fileName, true))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").PadRight(18));
                        stringBuilder.Append(level);
                        stringBuilder.Append(message);
                        file.WriteLine(stringBuilder.ToString());
                    }
                }
            };
            //Console.WriteLine(message);
            try
            {
                write(filePath + ".log");
            }
            catch (Exception)
            {
                write(filePath + Process.GetCurrentProcess().Id + ".log");
            }
        }

        #endregion Protected Methods
    }
}

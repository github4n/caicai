using System;
using System.Collections.Concurrent;

namespace Smart.Core.Logger
{
    /// <summary>
    /// 控制台日志
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {

        public ConsoleLogger()
        {
            UseConsole();
        }

        private static Boolean _useConsole;
        /// <summary>使用控制台输出日志，只能调用一次</summary>
        /// <param name="useColor">是否使用颜色，默认使用</param>
        /// <param name="useFileLog">是否同时使用文件日志，默认使用</param>
        public void UseConsole(Boolean useColor = true, Boolean useFileLog = true)
        {
            if (_useConsole) return;
            _useConsole = true;

            // 适当加大控制台窗口
            try
            {
                if (Console.WindowWidth <= 80) Console.WindowWidth = Console.WindowWidth * 3 / 2;
                if (Console.WindowHeight <= 25) Console.WindowHeight = Console.WindowHeight * 3 / 2;
            }
            catch { }
        }
        /// <summary>是否使用多种颜色，默认使用</summary>
        public Boolean UseColor { get; set; } = true;
        protected override void InputLogger(LogLevel level, string message)
        {
           
            var e = WriteLogEventArgs.Current.Set(level).Set(message, null);
            if (!UseColor)
            {
                ConsoleWriteLog(e, level);
                return;
            }
            lock (this)
            {
                var cc = Console.ForegroundColor;
                switch (level)
                {
                    case LogLevel.WARN:
                        cc = ConsoleColor.Yellow;
                        break;
                    case LogLevel.ERROR:
                    case LogLevel.FATAL:
                        cc = ConsoleColor.Red;
                        break;
                    default:
                        cc = GetColor(e.ThreadID);
                        break;
                }
                //var old = Console.ForegroundColor;
                Console.ForegroundColor = cc;
                ConsoleWriteLog(e, level);
            }            
        }


        private void ConsoleWriteLog(WriteLogEventArgs e, LogLevel level)
        {
            var msg = e.ToString();
            //Console.WriteLine(msg);
            Console.WriteLine($"{level.ToString()}--{msg}");
            Console.ReadLine();
        }

        static ConcurrentDictionary<Int32, ConsoleColor> dic = new ConcurrentDictionary<Int32, ConsoleColor>();
        static ConsoleColor[] colors = new ConsoleColor[] {
            ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Yellow,
            ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow };
        private ConsoleColor GetColor(Int32 threadid)
        {
            if (threadid == 1) return ConsoleColor.Gray;

            return dic.GetOrAdd(threadid, k => colors[dic.Count % colors.Length]);
        }

        /// <summary>已重载。</summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("{0} UseColor={1}", GetType().Name, UseColor);
        }
    }
}

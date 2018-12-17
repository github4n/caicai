using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace Smart.Core.Logger
{
    public class Log4Logger : LoggerBase
    {
        #region Public Constructors

        public Log4Logger(LoggerConfig loggerConfig)
        {
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, loggerConfig.Log4ConfigFileName)));
            projectName = loggerConfig.ProjectName;
            InstanceError = LogManager.GetLogger(repository.Name, "ErrorLog");
            InstanceFatal = LogManager.GetLogger(repository.Name, "FatalLog");
            InstanceInfo = LogManager.GetLogger(repository.Name, "InfoLog");
            InstanceWarn = LogManager.GetLogger(repository.Name, "WarnLog");
            InstanceDebug = LogManager.GetLogger(repository.Name, "DebugLog");
        }

        #endregion Public Constructors

        #region Private Fields

        private readonly ILog InstanceDebug;
        private readonly ILog InstanceError;
        private readonly ILog InstanceFatal;
        private readonly ILog InstanceInfo;
        private readonly ILog InstanceWarn;
        private ILoggerRepository repository;

        #endregion Private Fields

        #region Public Methods

        protected override void InputLogger(LogLevel level, string message)
        {
            message = $"{message}";
            switch (level)
            {
                case LogLevel.DEBUG:
                    if (InstanceDebug.IsDebugEnabled)
                    {
                        InstanceDebug.Debug(FormatStr("Debug", message, null));
                    }
                    break;

                case LogLevel.ERROR:
                    if (InstanceError.IsErrorEnabled)
                    {
                        InstanceError.Error(FormatStr("Error", message, null));
                    }
                    break;

                case LogLevel.FATAL:
                    if (InstanceFatal.IsFatalEnabled)
                    {
                        InstanceFatal.Fatal(FormatStr("Fatal", message, null));
                    }
                    break;

                case LogLevel.INFO:
                    if (InstanceInfo.IsInfoEnabled)
                    {
                        InstanceInfo.Info(FormatStr("Info", message, null));
                    }
                    break;

                case LogLevel.WARN:
                    if (InstanceWarn.IsWarnEnabled)
                    {
                        InstanceWarn.Warn(FormatStr("Warn", message, null));
                    }
                    break;
            }
        }

        #endregion Public Methods
    }
}

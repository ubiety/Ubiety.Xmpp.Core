using System;
using Serilog;
using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.App
{
    public class SerilogManager : ILogManager
    {
        public ILog GetLogger(string name)
        {
            return new SerilogLogger(name);
        }

        private class SerilogLogger : ILog
        {
            private readonly string _name;
            private const string _messageTemplate = "{Name} :: {Message}";

            public SerilogLogger(string name)
            {
                _name = name;
                Serilog.Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("log.txt")
                    .CreateLogger();
            }

            public void Log(LogLevel level, object message)
            {
                Log(level, message.ToString());
            }

            public void Log(LogLevel level, Exception exception, object message)
            {
                LogException(level, exception, message.ToString());
            }

            private void Log(LogLevel level, string message)
            {
                switch (level)
                {
                    case LogLevel.Critical:
                        Serilog.Log.Fatal(_messageTemplate, _name, message);
                        break;
                    case LogLevel.Error:
                        Serilog.Log.Error(_messageTemplate, _name, message);
                        break;
                    case LogLevel.Warning:
                        Serilog.Log.Warning(_messageTemplate, _name, message);
                        break;
                    case LogLevel.Information:
                        Serilog.Log.Information(_messageTemplate, _name, message);
                        break;
                    case LogLevel.Debug:
                        Serilog.Log.Debug(_messageTemplate, _name, message);
                        break;
                }
            }

            private void LogException(LogLevel level, Exception exception, string message)
            {
                switch (level)
                {
                    case LogLevel.Critical:
                        Serilog.Log.Fatal(exception, _messageTemplate, _name, message);
                        break;
                    case LogLevel.Error:
                        Serilog.Log.Error(exception, _messageTemplate, _name, message);
                        break;
                    case LogLevel.Warning:
                        Serilog.Log.Warning(exception, _messageTemplate, _name, message);
                        break;
                    case LogLevel.Information:
                        Serilog.Log.Information(exception, _messageTemplate, _name, message);
                        break;
                    case LogLevel.Debug:
                        Serilog.Log.Debug(exception, _messageTemplate, _name, message);
                        break;
                }
            }
        }
    }
}
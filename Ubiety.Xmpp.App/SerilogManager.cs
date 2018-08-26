using System;
using Serilog;
using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.App
{
    public class SerilogManager : ILogManager
    {
        public ILog Get(string name)
        {
            return new SerilogLogger(name);
        }

        private class SerilogLogger : ILog
        {
            private readonly string _name;

            public SerilogLogger(string name)
            {
                _name = name;
                Serilog.Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
            }
            
            public void Log(LogLevel level, object message)
            {
                Log(level, message.ToString());
            }

            public void Log(LogLevel level, string message, params object[] args)
            {
                Log(level, Format(message, args));
            }

            public void Log(LogLevel level, Exception exception, object message)
            {
                throw new NotImplementedException();
            }

            public void Log(LogLevel level, Exception exception, string message, params object[] args)
            {
                throw new NotImplementedException();
            }

            private void Log(LogLevel level, string message)
            {
                switch (level)
                {
                    case LogLevel.Critical:
                        Serilog.Log.Fatal($"{_name} :: {message}");
                        break;
                    case LogLevel.Error:
                        Serilog.Log.Error($"{_name} :: {message}");
                        break;
                    case LogLevel.Warning:
                        Serilog.Log.Warning($"{_name} :: {message}");
                        break;
                    case LogLevel.Information:
                        Serilog.Log.Information($"{_name} :: {message}");
                        break;
                    case LogLevel.Debug:
                        Serilog.Log.Debug($"{_name} :: {message}");
                        break;
                }
            }

            private static string Format(string format, object[] args)
            {
                return args != null && args.Length != 0 ? string.Format(format, args) : format;
            }
        }
    }
}

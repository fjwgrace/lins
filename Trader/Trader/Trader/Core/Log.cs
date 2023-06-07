using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.File;
namespace Trader.Core
{
    public sealed class Log
    {
        private static readonly ILogger _logger;
        static string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);
        static Log()
        {
            _logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .WriteTo.File("Logs//log.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate:SerilogOutputTemplate,
                retainedFileCountLimit: 31,
                retainedFileTimeLimit: TimeSpan.FromDays(2),
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 5242880 // 5MB
                )
  .CreateLogger();
        }
        public static ILogger Logger { get { return _logger;} }
    }
}

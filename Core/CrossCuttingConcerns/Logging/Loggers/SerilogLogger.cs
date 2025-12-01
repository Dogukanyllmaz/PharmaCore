using Serilog;

namespace Core.CrossCuttingConcerns.Logging.Loggers
{
    public class SerilogLogger : ILoggerService
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/pharma_.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        public void Info(string message) => _logger.Information(message);
        public void Debug(string message) => _logger.Debug(message);
        public void Warning(string message) => _logger.Warning(message);
        public void Error(string message) => _logger.Error(message);
        public void Fatal(string message) => _logger.Fatal(message);
        public void Log(object logDetail) => _logger.Information("{@LogDetail}", logDetail);
    }
}

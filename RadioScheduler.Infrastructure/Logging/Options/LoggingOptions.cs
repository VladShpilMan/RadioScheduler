namespace RadioScheduler.Infrastructure.Logging.Options
{
    public class LoggingOptions
    {
        public string? LoggerType { get; set; }
        public FileLoggerOptions FileLogger { get; set; } = new();
    }

    public class FileLoggerOptions
    {
        public string? Path { get; set; }
    }
}

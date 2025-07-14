using RadioScheduler.Infrastructure.Logging.Interfaces;

namespace RadioScheduler.Infrastructure.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public async Task LogErrorAsync(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ERROR: {message}";
                await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write to log file");
            }
        }
    }
}

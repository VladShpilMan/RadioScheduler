using RadioScheduler.Infrastructure.Logging;

namespace RadioScheduler.Tests.Logging
{
    public class FileLoggerTests : IDisposable
    {
        private readonly string _tempLogFilePath;

        public FileLoggerTests()
        {
            _tempLogFilePath = Path.Combine(Path.GetTempPath(), $"test_error_logs_{Guid.NewGuid()}.txt");
        }

        [Fact]
        public async Task LogErrorAsync_WritesErrorMessageToFile()
        {
            // Arrange
            FileLogger logger = new FileLogger(_tempLogFilePath);
            var errorMessage = "Test error message";

            // Act
            await logger.LogErrorAsync(errorMessage);

            // Assert
            string logContent = await File.ReadAllTextAsync(_tempLogFilePath);
            Assert.Contains($"ERROR: {errorMessage}", logContent);
            Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logContent);
        }

        [Fact]
        public async Task LogErrorAsync_FileWriteFails_WritesToConsole()
        {
            // Arrange
            string invalidPath = Path.Combine(Path.GetTempPath(), "\0invalid\0path.txt");
            FileLogger logger = new FileLogger(invalidPath);
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            await logger.LogErrorAsync("Test error message");

            // Assert
            string output = sw.ToString();
            Assert.Contains("Failed to write to log file", output);
        }

        public void Dispose()
        {
            if (File.Exists(_tempLogFilePath))
            {
                File.Delete(_tempLogFilePath);
            }
        }
    }
}

using RadioScheduler.Domain.Entities;
using RadioScheduler.Infrastructure.Services;

namespace RadioScheduler.Tests.Services
{
    public class ConsoleNotificationServiceTests
    {
        [Fact]
        public async Task NotifyShowCreatedAsync_WritesCorrectMessage()
        {
            // Arrange
            Show show = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            ConsoleNotificationService service = new ConsoleNotificationService();
            using StringWriter sw = new StringWriter();
            TextWriter originalConsoleOut = Console.Out;
            Console.SetOut(sw);

            try
            {
                // Act
                await service.NotifyShowCreatedAsync(show);

                // Assert
                string output = sw.ToString().Trim();

                Assert.Contains("Nowa audycja: Poranny Program o", output);
                Assert.Contains("14.07.2025", output);
                Assert.Contains("08:00:00", output);
                Assert.Contains("prowadzona przez: Jan Kowalski", output);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }
        }
    }
}
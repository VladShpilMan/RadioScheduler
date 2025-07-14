using Moq;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Application.Queries.GetDailyReport;
using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Tests.Queries
{
    public class GetDailyReportQueryHandlerTests
    {
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly GetDailyReportQueryHandler _handler;

        public GetDailyReportQueryHandlerTests()
        {
            _showRepositoryMock = new Mock<IShowRepository>();
            _handler = new GetDailyReportQueryHandler(_showRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingShows_ReturnsCorrectTotalDuration()
        {
            // Arrange
            DateTime date = new DateTime(2025, 7, 14);
            List<Show> shows = new List<Show>
            {
                new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60),
                new Show("Wieczorny Program", "Anna Nowak", new DateTime(2025, 7, 14, 20, 0, 0), 30)
            };
            _showRepositoryMock.Setup(r => r.GetShowsByDateAsync(date))
                .ReturnsAsync(shows);
            GetDailyReportQuery query = new GetDailyReportQuery { Date = date };

            // Act
            DailyReportDto result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(date, result.Date);
            Assert.Equal(2, result.TotalShows);
            Assert.Equal(90, result.TotalDurationMinutes);
            Assert.Equal(2, result.Shows?.Count());
            Assert.NotNull(result.Shows);
            Assert.Contains(result.Shows, s => s.Title == "Poranny Program");
            Assert.Contains(result.Shows, s => s.Title == "Wieczorny Program");
        }

        [Fact]
        public async Task Handle_NoShows_ReturnsEmptyReport()
        {
            // Arrange
            DateTime date = new DateTime(2025, 7, 14);
            _showRepositoryMock.Setup(r => r.GetShowsByDateAsync(date))
                .ReturnsAsync(new List<Show>());
            GetDailyReportQuery query = new GetDailyReportQuery { Date = date };

            // Act
            DailyReportDto result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(date, result.Date);
            Assert.Equal(0, result.TotalShows);
            Assert.Equal(0, result.TotalDurationMinutes);
            Assert.NotNull(result.Shows);
            Assert.Empty(result.Shows);
        }
    }
}

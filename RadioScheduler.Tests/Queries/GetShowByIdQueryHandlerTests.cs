using Moq;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Application.Queries.GetShowById;
using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Tests.Queries
{
    public class GetShowByIdQueryHandlerTests
    {
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly GetShowByIdQueryHandler _handler;

        public GetShowByIdQueryHandlerTests()
        {
            _showRepositoryMock = new Mock<IShowRepository>();
            _handler = new GetShowByIdQueryHandler(_showRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingShow_ReturnsShowDto()
        {
            // Arrange
            Guid showId = Guid.NewGuid();
            Show show = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            _showRepositoryMock.Setup(r => r.GetByIdAsync(showId))
                .ReturnsAsync(show);
            GetShowByIdQuery query = new GetShowByIdQuery { Id = showId };

            // Act
            ShowDto result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Poranny Program", result.Title);
            Assert.Equal("Jan Kowalski", result.Presenter);
            Assert.Equal(new DateTime(2025, 7, 14, 8, 0, 0), result.StartTime);
            Assert.Equal(60, result.DurationMinutes);
        }
    }
}

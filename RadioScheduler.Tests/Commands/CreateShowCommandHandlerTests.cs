using Moq;
using RadioScheduler.Application.Commands;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;
using RadioScheduler.Domain.Exceptions;

namespace RadioScheduler.Tests.Commands
{
    public class CreateShowCommandHandlerTests
    {
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly CreateShowCommandHandler _handler;

        public CreateShowCommandHandlerTests()
        {
            _showRepositoryMock = new Mock<IShowRepository>();
            _notificationServiceMock = new Mock<INotificationService>();

            _handler = new CreateShowCommandHandler(
                _showRepositoryMock.Object,
                _notificationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesShowAndNotifies()
        {
            // Arrange
            CreateShowCommand command = new CreateShowCommand
            {
                Title = "Poranny Program",
                Presenter = "Jan Kowalski",
                StartTime = new DateTime(2025, 7, 14, 8, 0, 0),
                DurationMinutes = 60
            };

            _showRepositoryMock.Setup(r => r.GetShowsByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Show>());
            _showRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Show>()))
                .Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(n => n.NotifyShowCreatedAsync(It.IsAny<Show>()))
                .Returns(Task.CompletedTask);

            // Act
            Guid result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _showRepositoryMock.Verify(r => r.AddAsync(It.Is<Show>(s =>
                s.Title == "Poranny Program" &&
                s.Presenter == "Jan Kowalski" &&
                s.StartTime == command.StartTime &&
                s.DurationMinutes == command.DurationMinutes)), Times.Once);

            _notificationServiceMock.Verify(n => n.NotifyShowCreatedAsync(It.Is<Show>(s =>
                s.Title == "Poranny Program" && s.Presenter == "Jan Kowalski")), Times.Once);
        }

        [Fact]
        public async Task Handle_CollisionDetected_ThrowsBusinessException()
        {
            // Arrange
            CreateShowCommand command = new CreateShowCommand
            {
                Title = "Wieczorny Program",
                Presenter = "Anna Nowak",
                StartTime = new DateTime(2025, 7, 14, 8, 30, 0),
                DurationMinutes = 60
            };

            Show existingShow = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);

            _showRepositoryMock.Setup(r => r.GetShowsByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Show> { existingShow });

            // Act & Assert
            BusinessException exception = await Assert.ThrowsAsync<BusinessException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Show time collision detected", exception.Message);
        }
    }
}

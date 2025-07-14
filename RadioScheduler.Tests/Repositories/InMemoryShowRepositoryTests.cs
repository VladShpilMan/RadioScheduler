using RadioScheduler.Domain.Entities;
using RadioScheduler.Infrastructure.Repositories;

namespace RadioScheduler.Tests.Repositories
{
    public class InMemoryShowRepositoryTests
    {
        private readonly InMemoryShowRepository _repository;

        public InMemoryShowRepositoryTests()
        {
            _repository = new InMemoryShowRepository();
        }

        [Fact]
        public async Task AddAsync_GetByIdAsync_ReturnsAddedShow()
        {
            // Arrange
            Show show = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);

            // Act
            await _repository.AddAsync(show);
            Show? result = await _repository.GetByIdAsync(show.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(show.Id, result.Id);
            Assert.Equal(show.Title, result.Title);
            Assert.Equal(show.Presenter, result.Presenter);
            Assert.Equal(show.StartTime, result.StartTime);
            Assert.Equal(show.DurationMinutes, result.DurationMinutes);
        }

        [Fact]
        public async Task GetShowsByDateAsync_ReturnsShowsForDate()
        {
            // Arrange
            Show show1 = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            Show show2 = new Show("Wieczorny Program", "Jan Kowalski", new DateTime(2025, 7, 15, 20, 0, 0), 30);
            await _repository.AddAsync(show1);
            await _repository.AddAsync(show2);

            // Act
            IEnumerable<Show> result = await _repository.GetShowsByDateAsync(new DateTime(2025, 7, 14));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(show1.Id, result.First().Id);
        }
    }
}

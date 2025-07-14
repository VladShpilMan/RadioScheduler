using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Tests.Entities
{
    public class ShowTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesShow()
        {
            // Arrange
            string title = "Poranny Program";
            string presenter = "Jan Kowalski";
            DateTime startTime = new DateTime(2025, 7, 14, 8, 0, 0);
            int durationMinutes = 60;

            // Act
            Show show = new Show(title, presenter, startTime, durationMinutes);

            // Assert
            Assert.NotEqual(Guid.Empty, show.Id);
            Assert.Equal(title, show.Title);
            Assert.Equal(presenter, show.Presenter);
            Assert.Equal(startTime, show.StartTime);
            Assert.Equal(durationMinutes, show.DurationMinutes);
            Assert.Equal(startTime.AddMinutes(durationMinutes), show.EndTime);
        }

        [Fact]
        public void Constructor_EmptyTitle_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Show("", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60));
            Assert.Throws<ArgumentException>(() =>
                new Show(null, "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60));
        }

        [Fact]
        public void Constructor_EmptyPresenter_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Show("Jan Kowalski", "", new DateTime(2025, 7, 14, 8, 0, 0), 60));
            Assert.Throws<ArgumentException>(() =>
                new Show("Jan Kowalski", null, new DateTime(2025, 7, 14, 8, 0, 0), 60));
        }

        [Fact]
        public void Constructor_NonPositiveDuration_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 0));
            Assert.Throws<ArgumentException>(() =>
                new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), -10));
        }

        [Fact]
        public void HasCollision_OverlappingShows_ReturnsTrue()
        {
            // Arrange
            Show show1 = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            Show show2 = new Show("Wieczorny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 30, 0), 60);

            // Act
            var result = show1.HasCollision(show2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasCollision_NonOverlappingShows_ReturnsFalse()
        {
            // Arrange
            Show show1 = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            Show show2 = new Show("Wieczorny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 10, 0, 0), 60);

            // Act
            bool result = show1.HasCollision(show2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasCollision_DifferentDates_ReturnsFalse()
        {
            // Arrange
            Show show1 = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);
            Show show2 = new Show("Poranny Program kolejnego dnia", "Jan Kowalski", new DateTime(2025, 7, 15, 8, 0, 0), 60);

            // Act
            bool result = show1.HasCollision(show2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasCollision_NullShow_ReturnsFalse()
        {
            // Arrange
            Show show = new Show("Poranny Program", "Jan Kowalski", new DateTime(2025, 7, 14, 8, 0, 0), 60);

            // Act
            bool result = show.HasCollision(null);

            // Assert
            Assert.False(result);
        }
    }
}

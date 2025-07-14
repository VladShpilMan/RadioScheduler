namespace RadioScheduler.Domain.Entities
{
    public class Show
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Presenter { get; private set; }
        public DateTime StartTime { get; private set; }
        public int DurationMinutes { get; private set; }

        public Show(string? title, string? presenter, DateTime startTime, int durationMinutes)
        {
            Id = Guid.NewGuid();
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be empty", nameof(title)) : title;
            Presenter = string.IsNullOrWhiteSpace(presenter) ? throw new ArgumentException("Presenter cannot be empty", nameof(presenter)) : presenter;
            StartTime = startTime;
            DurationMinutes = durationMinutes <= 0 ? throw new ArgumentException("Duration must be positive", nameof(durationMinutes)) : durationMinutes;
        }

        public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);

        public bool HasCollision(Show? other)
        {
            if (other == null || other.StartTime.Date != StartTime.Date)
                return false;

            return StartTime < other.EndTime && other.StartTime < EndTime;
        }
    }
}

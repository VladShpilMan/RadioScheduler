namespace RadioScheduler.Application.DTOs
{
    public class DailyReportDto
    {
        public DateTime Date { get; set; }
        public int TotalShows { get; set; }
        public int TotalDurationMinutes { get; set; }
        public IEnumerable<ShowDto>? Shows { get; set; }
    }
}

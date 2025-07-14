using MediatR;

namespace RadioScheduler.Application.Commands
{
    public class CreateShowCommand : IRequest<Guid>
    {
        public string? Title { get; set; }
        public string? Presenter { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}

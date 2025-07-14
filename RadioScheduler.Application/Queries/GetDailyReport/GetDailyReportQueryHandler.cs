using MediatR;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Application.Queries.GetDailyReport
{
    public class GetDailyReportQueryHandler : IRequestHandler<GetDailyReportQuery, DailyReportDto>
    {
        private readonly IShowRepository _showRepository;

        public GetDailyReportQueryHandler(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<DailyReportDto> Handle(GetDailyReportQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Show> shows = await _showRepository.GetShowsByDateAsync(request.Date);
            List<Show> sortedShows = shows.OrderBy(s => s.StartTime).ToList();

            return new DailyReportDto
            {
                Date = request.Date,
                TotalShows = sortedShows.Count,
                TotalDurationMinutes = sortedShows.Sum(s => s.DurationMinutes),
                Shows = sortedShows.Select(s => new ShowDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Presenter = s.Presenter,
                    StartTime = s.StartTime,
                    DurationMinutes = s.DurationMinutes
                }).ToList()
            };
        }
    }
}

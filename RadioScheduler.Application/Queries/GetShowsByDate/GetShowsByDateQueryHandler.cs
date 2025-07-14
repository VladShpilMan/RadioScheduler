using MediatR;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Application.Queries.GetShowsByDate
{
    public class GetShowsByDateQueryHandler : IRequestHandler<GetShowsByDateQuery, IEnumerable<ShowDto>>
    {
        private readonly IShowRepository _showRepository;

        public GetShowsByDateQueryHandler(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<IEnumerable<ShowDto>> Handle(GetShowsByDateQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Show> shows = await _showRepository.GetShowsByDateAsync(request.Date);
            return shows.Select(s => new ShowDto
            {
                Id = s.Id,
                Title = s.Title,
                Presenter = s.Presenter,
                StartTime = s.StartTime,
                DurationMinutes = s.DurationMinutes
            });
        }
    }
}

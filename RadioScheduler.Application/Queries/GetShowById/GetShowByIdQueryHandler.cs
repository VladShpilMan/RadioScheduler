using MediatR;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;
using RadioScheduler.Domain.Exceptions;

namespace RadioScheduler.Application.Queries.GetShowById
{
    public class GetShowByIdQueryHandler : IRequestHandler<GetShowByIdQuery, ShowDto>
    {
        private readonly IShowRepository _showRepository;

        public GetShowByIdQueryHandler(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<ShowDto> Handle(GetShowByIdQuery request, CancellationToken cancellationToken)
        {
            Show? show = await _showRepository.GetByIdAsync(request.Id);
            if (show == null)
                throw new BusinessException("Show not found");

            return new ShowDto
            {
                Id = show.Id,
                Title = show.Title,
                Presenter = show.Presenter,
                StartTime = show.StartTime,
                DurationMinutes = show.DurationMinutes
            };
        }
    }
}

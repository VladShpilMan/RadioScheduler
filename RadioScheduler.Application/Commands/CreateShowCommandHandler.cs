using MediatR;
using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;
using RadioScheduler.Domain.Exceptions;

namespace RadioScheduler.Application.Commands
{
    public class CreateShowCommandHandler : IRequestHandler<CreateShowCommand, Guid>
    {
        private readonly IShowRepository _showRepository;
        private readonly INotificationService _notificationService;

        public CreateShowCommandHandler(IShowRepository showRepository, INotificationService notificationService)
        {
            _showRepository = showRepository;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(CreateShowCommand request, CancellationToken cancellationToken)
        {
            Show show = new Show(request.Title, request.Presenter, request.StartTime, request.DurationMinutes);
            IEnumerable<Show> existingShows = await _showRepository.GetShowsByDateAsync(show.StartTime.Date);

            if (existingShows.Any(s => s.HasCollision(show)))
            {
                throw new BusinessException("Show time collision detected");
            }

            await _showRepository.AddAsync(show);
            await _notificationService.NotifyShowCreatedAsync(show);

            return show.Id;
        }
    }
}

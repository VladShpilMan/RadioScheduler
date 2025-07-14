using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyShowCreatedAsync(Show show);
    }
}

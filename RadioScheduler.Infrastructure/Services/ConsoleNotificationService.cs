using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;
using System.Diagnostics;

namespace RadioScheduler.Infrastructure.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        public async Task NotifyShowCreatedAsync(Show show)
        {
            Debug.WriteLine($"Nowa audycja: {show.Title} o {show.StartTime} prowadzona przez: {show.Presenter}");
            await Task.CompletedTask;
        }
    }
}

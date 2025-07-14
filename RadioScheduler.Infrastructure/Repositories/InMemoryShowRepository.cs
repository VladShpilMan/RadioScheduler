using RadioScheduler.Application.Interfaces;
using RadioScheduler.Domain.Entities;

namespace RadioScheduler.Infrastructure.Repositories
{
    public class InMemoryShowRepository : IShowRepository
    {
        private readonly List<Show> _shows = new();

        public async Task AddAsync(Show show)
        {
            _shows.Add(show);
            await Task.CompletedTask;
        }

        public async Task<Show?> GetByIdAsync(Guid id)
        {
            Show? show = _shows.FirstOrDefault(s => s.Id == id);
            return await Task.FromResult(show);
        }

        public async Task<IEnumerable<Show>> GetShowsByDateAsync(DateTime date)
        {
            List<Show> shows = _shows.Where(s => s.StartTime.Date == date.Date).ToList();
            return await Task.FromResult(shows);
        }
    }
}

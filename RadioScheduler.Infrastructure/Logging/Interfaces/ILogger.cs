namespace RadioScheduler.Infrastructure.Logging.Interfaces
{
    public interface ILogger
    {
        Task LogErrorAsync(string message);
    }
}

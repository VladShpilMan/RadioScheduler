using MediatR;
using RadioScheduler.Application.DTOs;

namespace RadioScheduler.Application.Queries.GetShowsByDate
{
    public class GetShowsByDateQuery : IRequest<IEnumerable<ShowDto>>
    {
        public DateTime Date { get; set; }
    }
}

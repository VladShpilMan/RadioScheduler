using MediatR;
using RadioScheduler.Application.DTOs;

namespace RadioScheduler.Application.Queries.GetShowById
{
    public class GetShowByIdQuery : IRequest<ShowDto>
    {
        public Guid Id { get; set; }
    }
}

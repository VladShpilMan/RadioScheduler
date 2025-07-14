using MediatR;
using RadioScheduler.Application.DTOs;

namespace RadioScheduler.Application.Queries.GetDailyReport
{
    public class GetDailyReportQuery : IRequest<DailyReportDto>
    {
        public DateTime Date { get; set; }
    }
}

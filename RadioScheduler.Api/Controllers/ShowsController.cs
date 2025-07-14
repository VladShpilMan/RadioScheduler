using MediatR;
using Microsoft.AspNetCore.Mvc;
using RadioScheduler.Application.Commands;
using RadioScheduler.Application.DTOs;
using RadioScheduler.Application.Queries.GetDailyReport;
using RadioScheduler.Application.Queries.GetShowById;
using RadioScheduler.Application.Queries.GetShowsByDate;

namespace RadioScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShow([FromBody] CreateShowCommand command)
        {
            Guid showId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetShowById), new { id = showId }, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowsByDate([FromQuery] DateTime date)
        {
            IEnumerable<ShowDto> shows = await _mediator.Send(new GetShowsByDateQuery { Date = date });
            return Ok(shows);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(Guid id)
        {
            ShowDto show = await _mediator.Send(new GetShowByIdQuery { Id = id });
            return Ok(show);
        }

        [HttpGet("daily-report")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
        {
            DailyReportDto report = await _mediator.Send(new GetDailyReportQuery { Date = date });
            return Ok(report);
        }
    }
}

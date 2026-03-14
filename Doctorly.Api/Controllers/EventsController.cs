using Doctorly.Application.Commands;
using Doctorly.Application.Queries;
using Doctorly.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Doctorly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly CreateEventCommandHandler _createHandler;
    private readonly GetEventQueryHandler _getQueryHandler;

    public EventsController(CreateEventCommandHandler createHandler, GetEventQueryHandler getQueryHandler)
    {
        _createHandler = createHandler;
        _getQueryHandler = getQueryHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEventResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateEventCommand command, CancellationToken ct)
    {
        var eventId = await _createHandler.Handle(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = eventId }, new CreateEventResponse(eventId));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CalendarEventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getQueryHandler.Handle(new GetEventQuery(id), ct);
        return result != null ? Ok(result) : NotFound();
    }
}

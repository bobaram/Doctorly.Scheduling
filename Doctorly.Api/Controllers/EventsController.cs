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
    private readonly GetEventsInRangeQueryHandler _listHandler;
    private readonly FindEventsByKeywordQueryHandler _searchHandler;
    private readonly UpdateEventCommandHandler _updateHandler;
    private readonly DeleteEventCommandHandler _deleteHandler;
    private readonly UpdateAttendeeStatusCommandHandler _statusHandler;

    public EventsController(
        CreateEventCommandHandler createHandler, 
        GetEventQueryHandler getQueryHandler,
        GetEventsInRangeQueryHandler listHandler,
        FindEventsByKeywordQueryHandler searchHandler,
        UpdateEventCommandHandler updateHandler,
        DeleteEventCommandHandler deleteHandler,
        UpdateAttendeeStatusCommandHandler statusHandler)
    {
        _createHandler = createHandler;
        _getQueryHandler = getQueryHandler;
        _listHandler = listHandler;
        _searchHandler = searchHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _statusHandler = statusHandler;
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CalendarEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var result = await _listHandler.Handle(new GetEventsInRangeQuery(from, to), ct);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<CalendarEventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string keyword, CancellationToken ct)
    {
        var result = await _searchHandler.Handle(new FindEventsByKeywordQuery(keyword), ct);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(Guid id, UpdateEventCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        await _updateHandler.Handle(command, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _deleteHandler.Handle(new DeleteEventCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id}/attendees/{email}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatus(Guid id, string email, [FromQuery] bool isAttending, CancellationToken ct)
    {
        await _statusHandler.Handle(new UpdateAttendeeStatusCommand(id, email, isAttending), ct);
        return NoContent();
    }
}

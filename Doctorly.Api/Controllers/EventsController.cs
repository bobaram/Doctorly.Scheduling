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

    public EventsController(
        CreateEventCommandHandler createHandler, 
        GetEventQueryHandler getQueryHandler,
        GetEventsInRangeQueryHandler listHandler,
        FindEventsByKeywordQueryHandler searchHandler)
    {
        _createHandler = createHandler;
        _getQueryHandler = getQueryHandler;
        _listHandler = listHandler;
        _searchHandler = searchHandler;
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
}

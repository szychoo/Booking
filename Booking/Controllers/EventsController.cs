using Booking.BusinessLogic;
using Booking.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsManager _eventsManager;

        public EventsController(IEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        // GET: api/Events?country={country}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetEventBasicResponse>>> GetAll([FromQuery] string? country)
        {
            var response = await _eventsManager.GetAllEvents(country);
            return Ok(response);
        }

        // GET: api/Events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetEventResponse>>> GetById(int id)
        {
            var response = await _eventsManager.GetEventById(id);
            if(response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<CreateEventResponse>> CreateEvent([FromBody] CreateEventRequest request)
        {
            var response = await _eventsManager.AddEvent(request);
            if(response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: api/Events/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> EditEvent(int id, [FromBody] UpdateEventRequest request)
        {
            request.Id = id;
            var response = await _eventsManager.UpdateEvent(request);
            if (response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response);
        }

        // DELETE: api/Events/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var response = await _eventsManager.DeleteEvent(id);
            if(response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }
            return NoContent();
        }
    }
}

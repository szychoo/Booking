using Booking.BusinessLogic;
using Booking.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/Events/{eventId}/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IEventsRegistrationManager _eventsRegistrationManager;

        public RegistrationsController(IEventsRegistrationManager eventsRegistrationManager)
        {
            _eventsRegistrationManager = eventsRegistrationManager;
        }

        // GET: api/Events/{eventId}/Registrations/{registrationId}
        [HttpGet("{registrationId}")]
        public async Task<ActionResult<IEnumerable<GetEventResponse>>> GetById(int eventId, int registrationId)
        {
            var response = await _eventsRegistrationManager.GetEventRegistration(new GetRegistrationRequest { Id = registrationId, EventId = eventId });
            if (response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST: api/Events/{eventId}/Registration
        [HttpPost]
        public async Task<ActionResult<AddRegistrationResponse>> AddRegistration(int eventId, [FromBody] AddRegistrationRequest registrationRequest)
        {
            registrationRequest.EventId = eventId;
            var response = await _eventsRegistrationManager.AddEventRegistration(registrationRequest);
            if (response.HasError)
            {
                return BadRequest(response.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { eventId = response.EventId, registrationId = response.Id }, response);
        }
    }
}

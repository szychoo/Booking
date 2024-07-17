using Booking.DTO;

namespace Booking.BusinessLogic
{
    public interface IEventsManager
    {
        Task<CreateEventResponse> AddEvent(CreateEventRequest eventItem);
        Task<BaseResponse> DeleteEvent(int id);
        Task<IEnumerable<GetEventBasicResponse>> GetAllEvents(string country = null);
        Task<GetEventResponse?> GetEventById(int id);
        Task<UpdateEventResponse> UpdateEvent(UpdateEventRequest updateEventRequest);
    }
}
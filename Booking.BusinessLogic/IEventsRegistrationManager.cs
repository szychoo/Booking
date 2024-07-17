using Booking.DTO;

namespace Booking.BusinessLogic
{
    public interface IEventsRegistrationManager
    {
        Task<AddRegistrationResponse> AddEventRegistration(AddRegistrationRequest request);

        Task<AddRegistrationResponse?> GetEventRegistration(GetRegistrationRequest request);
    }
}
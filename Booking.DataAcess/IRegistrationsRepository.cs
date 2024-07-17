
namespace Booking.DataAcess
{
    public interface IRegistrationsRepository
    {
        Task<EventRegistration> AddAsync(EventRegistration eventRegistration);
        Task<EventRegistration?> GetAsync(EventRegistration eventRegistration);
        Task<IEnumerable<EventRegistration>> GetByEventIdAsync(int eventId);
        Task DeleteAsync(int id);
    }
}
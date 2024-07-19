using System.Linq.Expressions;

namespace Booking.DataAcess
{
    public interface IEventsRepository
    {
        Task<Event> AddEventAsync(Event eventItem);
        Task DeleteEventAsync(int id);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event?> GetByNameAsync(string name);
        Task<List<Event>> GetAllEventsAsync(Expression<Func<Event, bool>> predicate);
        void UpdateEventAsync(Event eventItem);
    }
}
namespace Booking.DataAcess
{
    public interface IEventsRepository
    {
        Task<Event> AddEventAsync(Event eventItem);
        Task DeleteEventAsync(int id);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event?> GetByNameAsync(string name);
        Task<List<Event>> GetAllEventsByCountryAsync(string country);
        void UpdateEventAsync(Event eventItem);
    }
}
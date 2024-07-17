using Microsoft.EntityFrameworkCore;

namespace Booking.DataAcess
{
    public class EventsRepository : IEventsRepository
    {
        private readonly BookingContext _context;

        public EventsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Event> AddEventAsync(Event eventItem)
        {
            _context.Events.Add(eventItem);
            return eventItem;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        //TODO could improve with predicate to make it generic
        public async Task<List<Event>> GetAllEventsByCountryAsync(string country)
        {
            return await _context.Events.Where(e => e.Country == country).ToListAsync();
        }

        public async Task<Event?> GetByNameAsync(string name)
        {
            return await _context.Events.Where(e => e.Name == name).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public void UpdateEventAsync(Event eventItem)
        {
            _context.Entry(eventItem).State = EntityState.Modified;
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
            }
        }
    }
}

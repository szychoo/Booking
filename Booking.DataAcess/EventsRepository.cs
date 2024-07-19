using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<List<Event>> GetAllEventsAsync(Expression<Func<Event, bool>> predicate)
        {
            return await _context.Events.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<Event?> GetByNameAsync(string name)
        {
            return await _context.Events.AsNoTracking().Where(e => e.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
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

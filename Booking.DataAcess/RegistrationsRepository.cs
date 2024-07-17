using Microsoft.EntityFrameworkCore;

namespace Booking.DataAcess
{
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private readonly BookingContext _context;

        public RegistrationsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<EventRegistration> AddAsync(EventRegistration eventRegistration)
        {
            _context.EventRegistrations.Add(eventRegistration);
            return eventRegistration;
        }

        public async Task<EventRegistration?> GetAsync(EventRegistration eventRegistration)
        {
            return await _context.EventRegistrations
                .Where(x => x.UserEmail == eventRegistration.UserEmail && x.EventId == eventRegistration.EventId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EventRegistration>> GetByEventIdAsync(int eventId)
        {
            return await _context.EventRegistrations
                .Where(x =>  x.EventId == eventId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var registration = await _context.EventRegistrations.FindAsync(id);
            if (registration != null)
            {
                _context.EventRegistrations.Remove(registration);
            }
        }
    }
}

namespace Booking.DataAcess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingContext _context;
        private EventsRepository _eventsRepository;
        private RegistrationsRepository _registrationsRepository;

        public UnitOfWork(BookingContext context)
        {
            _context = context;
        }

        public IEventsRepository EventsRepository
        {
            get
            {
                if (_eventsRepository == null)
                {
                    _eventsRepository = new EventsRepository(_context);
                }
                return _eventsRepository;
            }
        }

        public IRegistrationsRepository RegistrationsRepository
        {
            get
            {
                if (_registrationsRepository == null)
                {
                    _registrationsRepository = new RegistrationsRepository(_context);
                }
                return _registrationsRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

namespace Booking.DataAcess
{
    public interface IUnitOfWork : IDisposable
    {
        IEventsRepository EventsRepository { get; }
        IRegistrationsRepository RegistrationsRepository { get; }
        Task<int> SaveAsync();
    }
}

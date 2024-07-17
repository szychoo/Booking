namespace Booking.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public IEnumerable<CreateEventRequest> Events { get; set; }
    }
}

namespace Booking.DTO
{
    public class GetEventResponse : GetEventBasicResponse
    {
        public string Description { get; set; }
        public int NumberOfSeats { get; set; }
    }
}

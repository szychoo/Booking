namespace Booking.DTO
{
    public class UpdateEventResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public int NumberOfSeats { get; set; }
    }
}

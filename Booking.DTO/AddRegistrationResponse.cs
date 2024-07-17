namespace Booking.DTO
{
    public class AddRegistrationResponse : BaseResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserEmail { get; set; }
    }
}

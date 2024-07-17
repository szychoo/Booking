namespace Booking.DTO
{
    public class BaseResponse
    {
        public bool HasError { get; set; } = false;

        public string? ErrorMessage { get; set; }
    }
}

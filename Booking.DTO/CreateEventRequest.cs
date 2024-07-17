using System.ComponentModel.DataAnnotations;

namespace Booking.DTO
{
    public class CreateEventRequest
    {

        [StringLength(50, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string Country { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        [Range(0, 100, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int NumberOfSeats { get; set; }
    }
}

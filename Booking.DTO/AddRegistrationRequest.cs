using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Booking.DTO
{
    public class AddRegistrationRequest
    {
        [JsonIgnore]
        public int EventId { get; set; }

        [EmailAddress(ErrorMessage = "The User Email is not a valid email address.")]
        public string UserEmail { get; set; }
    }
}

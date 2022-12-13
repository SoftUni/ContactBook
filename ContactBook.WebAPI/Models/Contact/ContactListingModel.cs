using System.Text.Json.Serialization;

namespace ContactBook.WebAPI.Models.Contact
{
    public class ContactListingModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }

        public string? Comments { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Owner { get; set; }
    }
}

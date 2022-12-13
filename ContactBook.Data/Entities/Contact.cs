using System.ComponentModel.DataAnnotations;

namespace ContactBook.Data.Entities
{
    using static DataConstants;

    public class Contact
    {
        public int Id { get; init; }

        [MaxLength(MaxFirstNameLength)]
        public string FirstName { get; set; }

        [MaxLength(MaxLastNameLength)]
        public string LastName { get; set; }

        [MaxLength(MaxEmailLength)]
        public string Email { get; set; }

        [MaxLength(MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public DateTime DateCreated { get; set; }

        [MaxLength(MaxCommentsLength)]
        public string? Comments { get; set; }

        public string OwnerId { get; init; }

        public User Owner { get; init; }
    }
}

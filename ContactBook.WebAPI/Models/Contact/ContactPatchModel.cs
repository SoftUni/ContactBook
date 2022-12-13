using System.ComponentModel.DataAnnotations;

using ContactBook.Data;

namespace ContactBook.WebAPI.Models.Contact
{
    using static DataConstants;

    public class ContactPatchModel
    {
        [Display(Name = "First Name")]
        [StringLength(MaxFirstNameLength, MinimumLength = MinFirstNameLength)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(MaxLastNameLength, MinimumLength = MinLastNameLength)]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(MaxPhoneNumberLength, MinimumLength = MinPhoneNumberLength)]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = "Enter a valid phone number (without spaces).")]
        public string? PhoneNumber { get; set; }

        [StringLength(MaxCommentsLength)]
        public string? Comments { get; set; }
    }
}

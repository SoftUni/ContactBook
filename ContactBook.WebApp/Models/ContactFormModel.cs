using System.ComponentModel.DataAnnotations;

using ContactBook.Data;

namespace ContactBook.WebApp.Models
{
    using static DataConstants;

    public class ContactFormModel
    {
        [Display(Name = "First Name")]
        [StringLength(MaxFirstNameLength, MinimumLength = MinFirstNameLength, 
            ErrorMessage = "First name should be at least {2} characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(MaxLastNameLength, MinimumLength = MinLastNameLength,
            ErrorMessage = "Last name should be at least {2} characters long.")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(MaxPhoneNumberLength, MinimumLength = MinPhoneNumberLength,
            ErrorMessage = "Phone number should be between {1} and {2} characters long.")]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = "Enter a valid phone number (without spaces).")]
        public string PhoneNumber { get; set; }

        [StringLength(MaxCommentsLength)]
        public string? Comments { get; set; }
    }
}

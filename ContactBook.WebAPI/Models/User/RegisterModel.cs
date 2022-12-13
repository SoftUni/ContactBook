using System.ComponentModel.DataAnnotations;

using ContactBook.Data;

namespace ContactBook.WebAPI.Models.User
{
    using static DataConstants;
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(MaxFirstNameLength, MinimumLength = MinFirstNameLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(MaxLastNameLength, MinimumLength = MinLastNameLength)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = "Enter a valid phone number (without spaces).")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

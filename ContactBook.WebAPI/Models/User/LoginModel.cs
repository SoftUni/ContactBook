using System.ComponentModel.DataAnnotations;

namespace ContactBook.WebAPI.Models.User
{
    public class LoginModel
    {
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}

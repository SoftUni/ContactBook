#nullable disable

using System.ComponentModel.DataAnnotations;
using ContactBook.Data;
using ContactBook.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactBook.WebApp.Areas.Identity.Pages.Account
{
    using static DataConstants;
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
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


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                };

                bool isPhoneAlreadyRegistered = _userManager.Users.Any(item => item.PhoneNumber == Input.PhoneNumber);

                if (isPhoneAlreadyRegistered)
                {
                    ModelState.AddModelError(Input.PhoneNumber, "This phone number is already used.");
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    if(error.Code == "DuplicateUserName")
                    {
                        error.Description = "Email is already taken.";
                    }

                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ContactBook.Data;
using ContactBook.Data.Entities;
using ContactBook.WebAPI.Infrastructure;
using ContactBook.WebAPI.Models.Response;
using ContactBook.WebAPI.Models.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactBook.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private ContactBookDbContext dbContext;
        private UserManager<User> userManager;
        private readonly IConfiguration _configuration;

        public UsersController(ContactBookDbContext context,
            IConfiguration configuration)
        {
            this.dbContext = context;
            this._configuration = configuration;

            var userStore = new UserStore<User>(dbContext);
            var hasher = new PasswordHasher<User>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var factory = new LoggerFactory();
            var logger = new Logger<UserManager<User>>(factory);
            this.userManager = new UserManager<User>(
                userStore, null, hasher, null, null, normalizer, null, null, logger);
        }

        /// <summary>
        /// Logs a user in. (Default user credentials: guest@mail.com / guest)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/login
        ///     {
        ///        "email": "guest@mail.com",
        ///        "password": "guest"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">Returns "OK" with JWT token with expiration date.</response>
        /// <response code="401">Returns "Unauthorized" when username or password doesn't match.</response>    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                string jwtSecret = _configuration["JWT:Secret"];
                byte[] jwtSecretBytes = Encoding.UTF8.GetBytes(jwtSecret);
                var authSigningKey = new SymmetricSecurityKey(jwtSecretBytes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                return Ok(new ResponseWithToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized(new ResponseMsg { Message = "Invalid username or password!" });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/register
        ///     {
        ///         "email": "someUsername@mail.bg",
        ///         "firstName": "someName",
        ///         "lastName": "someLastName",
        ///         "phoneNumber": "+192088877744",
        ///         "password": "somePassword",
        ///         "confirmPassword": "somePassword"
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with "Success" status and "User created successfully! message".</response>
        /// <response code="400">Returns "Bad Request" when user already exists or user creation failed.</response>    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return BadRequest(new ResponseMsg
                { Message = "User already exists!" });

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new ResponseMsg
                { Message = "Password and Confirm Password don't match!" });

            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new ResponseMsg
                { Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseMsg { Message = "User created successfully!" });
        }

        /// <summary>
        /// Gets data about the currently logged-in user.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/users/myData
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with user data.</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated.</response>    
        [Authorize]
        [HttpGet("myData")]
        public IActionResult GetMyUserData()
        {
            var currentUser = this.dbContext.Users
                .FirstOrDefault(u => u.Email == this.User.Email());

            var currentUserModel = new UserModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                Email = currentUser.Email
            };

            return Ok(currentUserModel);
        }
    }
}

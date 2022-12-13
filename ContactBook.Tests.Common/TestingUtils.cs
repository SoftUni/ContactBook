using System.Collections.Generic;
using System.Security.Claims;

using ContactBook.Data.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Tests.Common
{
    public class TestingUtils
    {
        public static void AssignCurrentUserForController(Controller controller, User user)
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            ClaimsIdentity userIdentity = new ClaimsIdentity(userClaims);
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userPrincipal
                }
            };
        }
    }
}

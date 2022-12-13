using ContactBook.Data;
using ContactBook.WebApp.Infrastructure;
using ContactBook.WebApp.Models;

using Microsoft.AspNetCore.Mvc;

namespace ContactBook.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContactBookDbContext dbContext;

        public HomeController(ContactBookDbContext context)
            => this.dbContext = context;

        public IActionResult Index()
        {
            var homeModel = new HomeViewModel();
            homeModel.AllContactsCount = this.dbContext.Contacts.Count();

            if (this.User.Identity.IsAuthenticated)
            {
                homeModel.UserContactsCount = this.dbContext.Contacts
                    .Where(c => c.OwnerId == this.User.Id()).Count();

                var currentUser = this.dbContext.Users.Find(this.User.Id());
                homeModel.UserFullName = currentUser.FirstName + " " + currentUser.LastName;

                homeModel.UserContact = new ContactViewModel()
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Email = currentUser.Email,
                    PhoneNumber = currentUser.PhoneNumber
                };
            }

            return View(homeModel);
        }

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400)
            {
                return View("Error400");
            }

            if (statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}

using ContactBook.Data;
using ContactBook.Data.Entities;
using ContactBook.WebApp.Infrastructure;
using ContactBook.WebApp.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.WebApp.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ContactBookDbContext dbContext;
        public ContactsController(ContactBookDbContext dbContext) 
            => this.dbContext = dbContext;

        public IActionResult All()
        {
            var contacts = this.dbContext.Contacts
                .Where(c => c.OwnerId == this.User.Id())
                .OrderByDescending(c => c.DateCreated)
                .Select(c => new ContactViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Comments = c.Comments
                })
                .ToList();

            return View(contacts);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(ContactFormModel contactModel)
        {
            var userContacts = this.dbContext.Contacts.Where(c => c.OwnerId == this.User.Id());
            if (userContacts.Any(c => c.PhoneNumber == contactModel.PhoneNumber))
            {
                this.ModelState.AddModelError(nameof(contactModel.PhoneNumber),
                    "You already have a contact with the given phone number!");
            }

            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            var contact = new Contact()
            {
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                PhoneNumber = contactModel.PhoneNumber,
                Email = contactModel.Email,
                Comments = contactModel.Comments,
                DateCreated = DateTime.Now,
                OwnerId = this.User.Id()
            };
            this.dbContext.Contacts.Add(contact);
            this.dbContext.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            var contact = dbContext.Contacts.Find(id);
            if (contact == null)
            {
                // When contact with this id doesn't exist
                return BadRequest();
            }

            if (this.User.Id() != contact.OwnerId)
            {
                // When current user is not an owner
                return Unauthorized();
            }

            var contactModel = new ContactFormModel()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Comments = contact.Comments
            };

            return View(contactModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, ContactFormModel contactModel)
        {
            var contact = this.dbContext.Contacts.Find(id);
            if (contact == null)
            {
                return BadRequest();
            }

            if (this.User.Id() != contact.OwnerId)
            {
                // Not an owner -> return "Unauthorized"
                return Unauthorized();
            }

            // Get contacts of given user, except the current one
            var userContacts = this.dbContext.Contacts.Where(c => c.OwnerId == this.User.Id() && c.Id != id);
            if (userContacts.Any(c => c.PhoneNumber == contactModel.PhoneNumber))
            {
                this.ModelState.AddModelError(nameof(contactModel.PhoneNumber),
                    "You already have a contact with the given phone number!");
            }

            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            contact.FirstName = contactModel.FirstName;
            contact.LastName = contactModel.LastName;
            contact.Email = contactModel.Email;
            contact.PhoneNumber = contactModel.PhoneNumber;
            contact.Comments = contactModel.Comments;

            this.dbContext.SaveChanges();
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var contact = this.dbContext.Contacts.Find(id);
            if (contact == null)
            {
                return BadRequest();
            }

            if (this.User.Id() != contact.OwnerId)
            {
                // Not an owner -> return "Unauthorized"
                return Unauthorized();
            }

            this.dbContext.Contacts.Remove(contact);
            this.dbContext.SaveChanges();
            return RedirectToAction(nameof(All));
        }

        public IActionResult Search()
        {
            return View(new ContactSearchFormModel());
        }

        [HttpPost]
        public IActionResult Search(ContactSearchFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var contacts = this.dbContext
                .Contacts
                .Where(c => c.OwnerId == this.User.Id())
                .Select(c => new ContactViewModel()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Comments = c.Comments
                });

            var keyword = model.Keyword == null ? string.Empty : model.Keyword.Trim().ToLower();
            if (!String.IsNullOrEmpty(keyword) && !String.IsNullOrEmpty(keyword))
            {
                contacts = contacts
                .Where(t => t.FirstName.ToLower().Contains(keyword)
                    || t.LastName.ToLower().Contains(keyword)
                    || t.Email.ToLower().Contains(keyword)
                    || t.PhoneNumber.ToLower().Contains(keyword)
                    || t.Comments.ToLower().Contains(keyword));
            }

            model.Contacts = contacts;

            return View(model);
        }
    }
}

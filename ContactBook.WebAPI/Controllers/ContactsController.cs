using ContactBook.Data;
using ContactBook.Data.Entities;
using ContactBook.WebAPI.Infrastructure;
using ContactBook.WebAPI.Models.Contact;
using ContactBook.WebAPI.Models.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactBookDbContext dbContext;

        public ContactsController(ContactBookDbContext context)
            => this.dbContext = context;

        /// <summary>
        /// Gets contacts count.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/contacts/count
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with contacts count</response>  
        [HttpGet("count")]
        [AllowAnonymous]
        public IActionResult GetContactsCount()
        {
            var contactsModel = new ContactsCountModel();

            contactsModel.AllContactsCount = this.dbContext
                .Contacts
                .Count();

            if (this.User.Identity.IsAuthenticated)
            {
                contactsModel.UserContactsCount = this.dbContext
                    .Contacts
                    .Where(c => c.Owner.Email == this.User.Email())
                    .Count();
            }

            return Ok(contactsModel);
        }

        /// <summary>
        /// Gets a list with all contacts of the logged-in user.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/contacts
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with a list of all contacts</response>
        /// <response code="204">Returns "No Content" when user has no contacts</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>    
        [HttpGet()]
        public IActionResult GetContacts()
        {
            var contacts = this.dbContext
                .Contacts
                .Where(c => c.Owner.Email == this.User.Email())
                .OrderByDescending(c => c.DateCreated)
                .Select(c => CreateContactExtendedModel(c))
                .ToList();

            if (contacts.Count == 0)
            {
                return NoContent();
            }

            return Ok(contacts);
        }

        /// <summary>
        /// Gets contacts by keyword.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/contacts/search/{keyword}
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the matching contacts of the logged-in user</response>
        /// <response code="204">Returns "No Content" when there are no matching contacts of the logged-in user</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>
        [HttpGet("search/{keyword}")]
        public IActionResult GetContactsByKeyword(string? keyword)
        {
            var contacts = dbContext
                .Contacts
                .Where(c => c.Owner.Email == this.User.Email())
                .Select(c => CreateContactExtendedModel(c))
                .ToList();

            keyword = keyword == null ? string.Empty : keyword.Trim().ToLower();
            if (!String.IsNullOrEmpty(keyword) && !String.IsNullOrEmpty(keyword))
            {
                contacts = contacts
                    .Where(t => t.FirstName.ToLower().Contains(keyword)
                        || t.LastName.ToLower().Contains(keyword)
                        || t.Email.ToLower().Contains(keyword)
                        || t.PhoneNumber.ToLower().Contains(keyword)
                        || t.Comments.ToLower().Contains(keyword))
                    .ToList();
            }

            if (contacts.Count() == 0)
            {
                return NoContent();
            }

            return Ok(contacts);
        }

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     POST /api/contacts/create
        ///     {
        ///            "first name": "Johnny",
        ///            "last name": "Depp",
        ///            "email": "j.depp@mail.com",
        ///            "phone number": "+12298015369",
        ///            "comments": "An American actor, producer, and musician. Best in 'Pirates of the Caribbean'"
        ///     }
        /// </remarks>
        /// <response code="201">Returns "Created" with the created event</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response> 
        [HttpPost("create")]
        public IActionResult CreateContact(ContactBindingModel contactModel)
        {
            var userContacts = this.dbContext.Contacts
                .Where(c => c.Owner.Email == this.User.Email());

            if (userContacts.Any(c => c.PhoneNumber == contactModel.PhoneNumber))
            {
                return BadRequest(new ResponseMsg()
                {
                    Message = "You already have a contact with the given phone number!"
                });
            }

            var currentLoggedInUser = this.dbContext
                .Users
                .FirstOrDefault(u => u.Email == this.User.Email());

            var contact = new Contact()
            {
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                PhoneNumber = contactModel.PhoneNumber,
                Email = contactModel.Email,
                Comments = contactModel.Comments,
                DateCreated = DateTime.Now,
                OwnerId = currentLoggedInUser.Id
            };
            this.dbContext.Contacts.Add(contact);
            this.dbContext.SaveChanges();

            var resultModel = CreateContactExtendedModel(contact);
            resultModel.Owner = currentLoggedInUser.Email;

            return new ObjectResult(resultModel) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Edits a contact.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the edited contact!
        /// 
        /// Sample request:
        ///
        ///     PUT /api/contacts/{id}
        ///     {
        ///           "first name": "Johnny",
        ///           "last name": "Depp",
        ///           "email": "j.depp@mail.com",
        ///           "phone number": "+12298015369",
        ///           "comments": "An American actor, producer, and musician. Best in 'Pirates of the Caribbean'"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the contact</response>  
        /// <response code="404">Returns "Not Found" when contact with the given id doesn't exist</response>  
        [HttpPut("{id}")]
        public IActionResult PutContact(int id, ContactBindingModel contactModel)
        {
            var contactExists = this.dbContext.Contacts.Any(c => c.Id == id);
            if (!contactExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Contact with id #{id} not found." });
            }

            var contact = this.dbContext.Contacts
                .Include(c => c.Owner)
                .FirstOrDefault(c => c.Id == id);

            if (contact.Owner.Email != this.User.Email())
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot edit contact, when you are not its owner." });
            }

            // Get contacts of given user, except the current one
            var userContacts = this.dbContext.Contacts.Where(c => c.Owner.Email == this.User.Email() && c.Id != id);
            if (userContacts.Any(c => c.PhoneNumber == contactModel.PhoneNumber))
            {
                return BadRequest(new ResponseMsg()
                {
                    Message = "You already have a contact with the given phone number!"
                });
            }

            contact.FirstName = contactModel.FirstName;
            contact.LastName = contactModel.LastName;
            contact.Email = contactModel.Email;
            contact.PhoneNumber = contactModel.PhoneNumber;
            contact.Comments = contactModel.Comments;

            this.dbContext.SaveChanges();

            var editedContactModel = CreateContactExtendedModel(contact);
            return Ok(editedContactModel);
        }

        /// <summary>
        /// Partially edits a contact.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the edited contact!
        /// 
        /// Sample request:
        ///
        ///     PATCH /api/contact/{id}
        ///     {
        ///          "email": "johnny.depp@mail.com",
        ///          "phone number": "+12298015369"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the contact</response>  
        /// <response code="404">Returns "Not Found" when contact with the given id doesn't exist</response>
        [HttpPatch("{id}")]
        public IActionResult PatchContact(int id, ContactPatchModel contactModel)
        {
            var contactExists = this.dbContext.Contacts.Any(c => c.Id == id);
            if (!contactExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Contact with id #{id} not found." });
            }

            var contact = this.dbContext.Contacts
                .Include(c => c.Owner)
                .FirstOrDefault(c => c.Id == id);

            if (contact.Owner.Email != this.User.Email())
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot edit contact, when you are not its owner." });
            }

            // Get contacts of given user, except the current one
            var userContacts = this.dbContext.Contacts.Where(c => c.Owner.Email == this.User.Email() && c.Id != id);
            if (userContacts.Any(c => c.PhoneNumber == contactModel.PhoneNumber))
            {
                return BadRequest(new ResponseMsg()
                {
                    Message = "You already have a contact with the given phone number!"
                });
            }

            contact.FirstName = string.IsNullOrEmpty(contactModel.FirstName) ? contact.FirstName : contactModel.FirstName;
            contact.LastName = string.IsNullOrEmpty(contactModel.LastName) ? contact.LastName : contactModel.LastName;
            contact.Email = string.IsNullOrEmpty(contactModel.Email) ? contact.Email : contactModel.Email;
            contact.PhoneNumber = string.IsNullOrEmpty(contactModel.PhoneNumber) ? contact.PhoneNumber : contactModel.PhoneNumber;
            contact.Comments = contactModel.Email == null ? contact.Comments : contactModel.Comments;

            this.dbContext.SaveChanges();

            var editedContactModel = CreateContactExtendedModel(contact);
            return Ok(editedContactModel);
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the deleted contact!
        /// 
        /// Sample request:
        ///
        ///     DELETE /api/contacts/{id}
        ///     {
        ///            
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the deleted contact</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the contact</response>  
        /// <response code="404">Returns "Not Found" when contact with the given id doesn't exist</response> 
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contactExists = this.dbContext.Contacts.Any(c => c.Id == id);
            if (!contactExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Contact with id #{id} not found." });
            }

            var contact = this.dbContext.Contacts
                .Include(c => c.Owner)
                .FirstOrDefault(c => c.Id == id);

            if (contact.Owner.Email != this.User.Email())
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot delete contact, when you are not its owner." });
            }

            this.dbContext.Contacts.Remove(contact);
            this.dbContext.SaveChanges();

            var deletedContactModel = CreateContactExtendedModel(contact);
            return Ok(deletedContactModel);
        }

        private static ContactListingModel CreateContactExtendedModel(Contact contact)
            => new ContactListingModel()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Comments = contact.Comments
            };
    }
}

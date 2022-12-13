using System;
using System.Globalization;

using ContactBook.Data;
using ContactBook.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Tests.Common
{
    public class TestDb : IdentityDbContext<User>
    {
        private readonly string uniqueDbName;

        public TestDb()
        {
            this.uniqueDbName = $"ContactBook-TestDb-{DateTime.Now.Ticks}";
            this.SeedDatabase();
        }

        public ContactBookDbContext CreateDbContext()
        {
            var optionsBuilder
                = new DbContextOptionsBuilder<ContactBookDbContext>();

            optionsBuilder.UseInMemoryDatabase(this.uniqueDbName);
            return new ContactBookDbContext(optionsBuilder.Options, false);
        }

        public Contact SteveContact { get; set; }

        public Contact MichaelContact { get; set; }

        public Contact AlbertContact { get; set; }

        public User GuestUser { get; set; }

        public User UserMaria { get; set; }

        private void SeedDatabase()
        {
            ContactBookDbContext dbContext = this.CreateDbContext();
            UserStore<User> userStore = new UserStore<User>(dbContext);
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            UpperInvariantLookupNormalizer normalizer = new UpperInvariantLookupNormalizer();
            UserManager<User> userManager = new UserManager<User>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

            // Create GuestUser
            this.GuestUser = new User
            {
                UserName = "guest@mail.com",
                Email = "guest@mail.com",
                FirstName = "Guest",
                LastName = "User",
                PhoneNumber = "+359882634611"
            };
            userManager.CreateAsync(this.GuestUser, this.GuestUser.UserName).Wait();

            // MichaelContact has owner GuestUser
            this.MichaelContact = new Contact
            {
                Id = 2,
                FirstName = "Michael",
                LastName = "Jackson",
                Email = "michael@jackson.com",
                PhoneNumber = "+190088877744",
                DateCreated = DateTime.ParseExact("07-05-2022 10:20", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                Comments = "Michael Joseph Jackson was an American singer, songwriter, and dancer.",
                OwnerId = this.GuestUser.Id
            };
            dbContext.Add(this.MichaelContact);

            // AlbertContact has owner GuestUser
            this.AlbertContact = new Contact
            {
                Id = 3,
                FirstName = "Albert",
                LastName = "Einstein",
                Email = "albert.e@uzh.ch",
                PhoneNumber = "+41446344901",
                DateCreated = DateTime.ParseExact("11-11-2019 13:30", "dd-MM-yyyy HH:mm",
                    CultureInfo.InvariantCulture),
                Comments =
                    "Albert Einstein was a German-born theoretical physicist, universally acknowledged to be one of the two greatest physicists of all time, the other being Isaac Newton.",
                OwnerId = this.GuestUser.Id
            };
            dbContext.Add(this.AlbertContact);

            // Create UserMaria
            this.UserMaria = new User
            {
                UserName = "maria@gmail.com",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
                PhoneNumber = "+359882134611"
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            // SteveContact has owner UserMaria
            this.SteveContact = new Contact
            {
                Id = 1,
                FirstName = "Steve",
                LastName = "Jobs",
                Email = "steve@apple.com",
                PhoneNumber = "+180023456789",
                DateCreated = DateTime.ParseExact("29-05-2015 05:50", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                Comments =
                    "Steven Jobs was an American business magnate, industrial designer, investor, and media proprietor.",
                OwnerId = this.UserMaria.Id
            };
            dbContext.Add(this.SteveContact);

            dbContext.SaveChanges();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ContactBook.Data.Entities;
using ContactBook.WebAPI.Models.Contact;
using ContactBook.WebAPI.Models.User;

using Newtonsoft.Json;

using NUnit.Framework;

namespace ContactBook.WebAPI.IntegrationTests
{
    [TestFixture]
    public class ApiTestsWithUser : ApiTestsBase
    {
        private User maria;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await base.AuthenticateAsync();

            this.maria = this.testDb.UserMaria;
        }

        [Test]
        public async Task Test_Contacts_GetContactsSearch_ReturnsOk()
        {

        }

        [Test]
        public async Task Test_CreateContact_ShouldCreateCorrectly()
        {

        }

        [Test]
        public async Task Test_EditContact_ShouldEditCorrectly()
        {

        }
    }
}

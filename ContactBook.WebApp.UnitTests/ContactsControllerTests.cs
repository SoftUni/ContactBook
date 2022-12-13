using System;
using System.Net;
using System.Linq;

using ContactBook.Data;
using ContactBook.Tests.Common;
using ContactBook.WebApp.Controllers;
using ContactBook.WebApp.Models;

using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactBook.Data.Entities;
using ContactViewModel = ContactBook.WebApp.Models.ContactViewModel;

namespace ContactBook.WebApp.UnitTests
{
    public class ContactsControllerTests : UnitTestsBase
    {
        private ContactsController controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Instantiate the controller class with the testing database
            this.controller = new ContactsController(
                this.testDb.CreateDbContext());
            // Set UserMaria as current logged user
            TestingUtils.AssignCurrentUserForController(this.controller, this.testDb.UserMaria);
        }

        [Test]
        public void Test_All()
        {

        }

        [Test]
        public void Test_Create_PostInvalidData()
        {

        }

        [Test]
        public void Test_Delete_PostValidData()
        {

        }

        [Test]
        public void Test_Edit_UnauthorizedUser()
        {

        }
    }
}

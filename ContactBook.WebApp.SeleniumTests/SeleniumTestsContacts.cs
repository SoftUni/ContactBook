using System.Collections.ObjectModel;

using NUnit.Framework;

using OpenQA.Selenium;

namespace ContactBook.WebApp.SeleniumTests
{
    public class SeleniumTestsContacts : SeleniumTestsBase
    {
        [OneTimeSetUp]
        public void OneTimeSetUpUser()
        {
            this.RegisterUserForTesting();
        }

        [Test]
        public void Test_CreateContact_ValidData()
        {

        }

        [Test]
        public void Test_CreateContact_InvalidData()
        {

        }

        [Test]
        public void Test_DeleteContact()
        {

        }

        [Test]
        public void Test_EditTask()
        {

        }

        private void RegisterUserForTesting()
        {

        }

        private void CreateTask(out string contactFirstName, out string contactLastName)
        {

        }
    }
}

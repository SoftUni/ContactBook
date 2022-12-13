using System.Diagnostics;

using ContactBook.Tests.Common;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ContactBook.WebApp.SeleniumTests
{
    public class SeleniumTestsBase
    {
        protected TestDb testDb;
        protected IWebDriver driver;
        protected TestContactBookApp<Program> testContactBookApp;
        protected string baseUrl;
        protected string username;
        protected string firstName;
        protected string lastName;
        protected string password;
        protected string phoneNumber;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            // Run the Web app in a local Web server
            this.testDb = new TestDb();

            this.testContactBookApp = new TestContactBookApp<Program>(this.testDb);
            this.testContactBookApp.CreateClient();

            this.baseUrl = this.testContactBookApp.HostUrl;

            // Setup the user
            this.username = $"user{DateTime.Now.Ticks.ToString()[10..]}";
            this.firstName = "Pesho";
            this.lastName = "Petrov";
            this.password = $"pass{DateTime.Now.Ticks.ToString()[10..]}";
            this.phoneNumber = "+359886183166";

            // Setup the ChromeDriver
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!Debugger.IsAttached)
                chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("--start-maximized");
            this.driver = new ChromeDriver(chromeOptions);

            // Set an implicit wait for the UI interaction
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the Selenium driver
            this.driver.Quit();

            // Stop and dispose the local Web server
            this.testContactBookApp.Dispose();
        }
    }
}

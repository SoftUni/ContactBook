using ContactBook.Data;
using ContactBook.Tests.Common;

using NUnit.Framework;

namespace ContactBook.WebApp.UnitTests
{
    public class UnitTestsBase
    {
        protected TestDb testDb;
        protected ContactBookDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetupBase()
        {
            this.testDb = new TestDb();
            this.dbContext = this.testDb.CreateDbContext();
        }
    }
}

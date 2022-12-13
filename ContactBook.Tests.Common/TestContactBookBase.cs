using System.Linq;

using ContactBook.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ContactBook.Tests.Common
{
    public class TestContactBookBase<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        protected TestDb TestDb { get; set; }

        protected TestContactBookBase(TestDb testDb) 
            => this.TestDb = testDb;

        protected IWebHostBuilder ConfigureServices(IWebHostBuilder webHostBuilder)
            => webHostBuilder.ConfigureServices(services =>
            {
                ServiceDescriptor? oldDbContext = services.SingleOrDefault(
                        descr => descr.ServiceType == typeof(ContactBookDbContext));
                services.Remove(oldDbContext);
                services.AddScoped<ContactBookDbContext>(
                    provider => this.TestDb.CreateDbContext());
            });
    }
}

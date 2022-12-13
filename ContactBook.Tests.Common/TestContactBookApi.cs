using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace ContactBook.Tests.Common
{
    public class TestContactBookApi<TEntryPoint> : TestContactBookBase<TEntryPoint>
        where TEntryPoint : class
    {
        private WebApplicationFactory<TEntryPoint> factory;

        public TestContactBookApi(TestDb testDb)
            : base(testDb)
        {
        }

        public WebApplicationFactory<TEntryPoint> CreateFactory()
        {
            this.factory = new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(webHostBuilder =>
                    ConfigureServices(webHostBuilder));

            return this.factory;
        }

        public void Dispose() => this.factory.Dispose();
    }
}

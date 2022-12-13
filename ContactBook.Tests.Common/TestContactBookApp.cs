using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ContactBook.Tests.Common
{
    public class TestContactBookApp<TEntryPoint> : TestContactBookBase<TEntryPoint>
        where TEntryPoint : class
    {
        private IHost builtInHost;

        public TestContactBookApp(TestDb testDb)
            : base(testDb)
        {
        }

        public string HostUrl => "http://localhost:8080";

        protected override IHost CreateHost(IHostBuilder builder)
        {
            this.builtInHost = builder.Build();

            builder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder = ConfigureServices(webHostBuilder);
                webHostBuilder.UseUrls(HostUrl);
                webHostBuilder.UseKestrel();
            });

            IHost customHost = builder.Build();
            customHost.Start();

            return this.builtInHost;
        }

        public void Dispose() => this.builtInHost.Dispose();
    }
}

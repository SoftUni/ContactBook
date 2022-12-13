using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ContactBook.Data;
using ContactBook.Data.Entities;
using ContactBook.Tests.Common;
using ContactBook.WebAPI.Models.Response;
using ContactBook.WebAPI.Models.User;

using NUnit.Framework;

namespace ContactBook.WebAPI.IntegrationTests
{
    public class ApiTestsBase
    {
        protected TestDb testDb;
        protected ContactBookDbContext dbContext;
        protected TestContactBookApi<Program> testContactBookApi;
        protected HttpClient httpClient;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.testDb = new TestDb();
            this.dbContext = this.testDb.CreateDbContext();
            this.testContactBookApi = new TestContactBookApi<Program>(this.testDb);
            this.httpClient = this.testContactBookApi.CreateFactory().CreateClient();
        }

        public async Task AuthenticateAsync()
        {
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await this.GetJWTAsync());
        }

        private async Task<string> GetJWTAsync()
        {
            User userMaria = this.testDb.UserMaria;
            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("/api/users/login",
                new LoginModel
                {
                    Email = userMaria.Email,
                    Password = userMaria.UserName
                });

            ResponseWithToken loginResponse = await response.Content.ReadFromJsonAsync<ResponseWithToken>();

            return loginResponse.Token;
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the local Web API server
            this.testContactBookApi.Dispose();
        }
    }
}

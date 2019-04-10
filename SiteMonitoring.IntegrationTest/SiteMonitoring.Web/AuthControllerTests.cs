using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteMonitoring.IntegrationTest.Domain;
using SiteMonitoring.Model.Model.Requests;
using SiteMonitoring.Model.Model.Response;
using Xunit;

namespace SiteMonitoring.IntegrationTest.SiteMonitoring.Web
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this._client = factory.CreateClient();
        }

        [Fact]
        public async Task CheckAdminLogin()
        {
            var requestParams = JsonConvert.SerializeObject(new LoginRequestModel { Password = "admin", Login = "admin" });
            var requestContent = new StringContent(requestParams, Encoding.UTF8, "application/json");

            var httpResponse = await this._client.PostAsync("/api/Auth/Login", requestContent);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<LoginResponseModel>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(response.Success);
            Assert.True(response.Token?.Length != null);
        }
    }
}

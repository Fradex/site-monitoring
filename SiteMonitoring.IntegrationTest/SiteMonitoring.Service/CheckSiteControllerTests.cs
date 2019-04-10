using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteMonitoring.IntegrationTest.Domain;
using SiteMonitoring.Model.Model;
using Xunit;
using StartupService = SiteMonitoring.Service.Startup;

namespace SiteMonitoring.IntegrationTest.SiteMonitoring.Service
{
    public class CheckSiteControllerTests : IClassFixture<CustomWebApplicationFactory<StartupService>>
    {
        private readonly HttpClient _client;
        private SiteDTO _site;

        public CheckSiteControllerTests(CustomWebApplicationFactory<StartupService> factory)
        {
            this._client = factory.CreateClient();
            _site = new SiteDTO()
            {
                CheckedInterval = 5,
                Description = "Test",
                IsAvailable = false,
                LastUpdatedDate = DateTime.MaxValue,
                Name = "Test",
                Url = "https://google.com"
            };
        }

        [Fact]
        public async Task AddOrUpdateJobTest()
        {
            var requestParams = JsonConvert.SerializeObject(_site);
            var requestContent = new StringContent(requestParams, Encoding.UTF8, "application/json");
            var httpResponse = await this._client.PostAsync("/api/CheckSite/AddOrUpdateJob", requestContent);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<SiteDTO>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotEqual(res.LastUpdatedDate, DateTime.MinValue);
            Assert.NotNull(res.JobId);
            Assert.True(res.IsAvailable);

            _site = res;
        }


        [Fact]
        public async Task DeleteJobTest()
        {
            await AddOrUpdateJobTest();
            var httpResponse = await this._client.DeleteAsync($"/api/CheckSite/{this._site.JobId}");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }
    }
}

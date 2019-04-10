using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using SiteMonitoring.IntegrationTest.Domain;
using SiteMonitoring.Model.Model;
using Xunit;

namespace SiteMonitoring.IntegrationTest.SiteMonitoring.Web
{
    public class SiteControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private SiteDTO _site;

        public SiteControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            this._client = factory.CreateClient();
            _site = new SiteDTO()
            {
                CheckedInterval = 5,
                Description = "Test",
                IsAvailable = false,
                LastUpdatedDate = DateTime.MaxValue,
                Name = "Test",
                Url = "http://test.tu"
            };
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var httpResponse = await this._client.GetAsync("/api/Site");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var sites = JsonConvert.DeserializeObject<IEnumerable<SiteDTO>>(stringResponse);

            Assert.True(sites.Any());
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async Task SaveAsyncTest()
        {
            var requestParams = JsonConvert.SerializeObject(_site);
            var requestContent = new StringContent(requestParams, Encoding.UTF8, "application/json");
            var httpResponse = await this._client.PostAsync("/api/Site/Save", requestContent);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<SiteDTO>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(res.CheckedInterval, _site.CheckedInterval);
            Assert.False(res.IsAvailable);

            Assert.Equal(res.LastUpdatedDate, DateTime.MaxValue);
            Assert.Equal(res.Description, _site.Description);
            Assert.Equal(res.Name, _site.Name);
            Assert.Equal(res.Url, _site.Url);
            Assert.NotEqual(res.Id, Guid.Empty);
        }
    }
}

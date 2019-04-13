using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Service.Services
{
    /// <summary>
    /// Сервис для работы с объектами сайта
    /// </summary>
    public class SiteService : ISiteService
    {
        protected IConfiguration Configuration;
        protected ILogger<SiteService> Logger;

        public SiteService(ILogger<SiteService> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<SiteDTO>> GetAllSites()
        {
            var webApiUrl = Configuration.GetValue<string>("Hosts:SiteMonitoring.Web");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response =
                        await httpClient.GetAsync(
                                $"{webApiUrl}/api/Site")
                            .ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync()
                        .ConfigureAwait(false);

                    switch (response?.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        {
                            return JsonConvert.DeserializeObject<List<SiteDTO>>(responseText);
                        }
                        default:
                        {
                            throw new HttpRequestException($"StatusCode:{response.StatusCode}. ErrorText:{responseText}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Logger.LogError("Произошла ошибка при получении записей.", e);
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public async Task UpdateSiteStatus(SiteDTO model)
        {
            var webApiUrl = Configuration.GetValue<string>("Hosts:SiteMonitoring.Web");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var requestParams = JsonConvert.SerializeObject(model);
                    var requestContent = new StringContent(requestParams, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(
                            $"{webApiUrl}/api/Site/UpdateStatus", requestContent)
                        .ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    switch (response?.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        {
                            return;
                        }
                        default:
                        {
                            throw new HttpRequestException($"StatusCode:{response.StatusCode}. ErrorText:{responseText}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Logger.LogError("Произошла ошибка при получении записей.", e);
                    throw;
                }
            }
        }
    }
}

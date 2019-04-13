using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Services.Interfaces;

namespace SiteMonitoring.Services
{
    /// <summary>
    /// Сервис для вызова методов сервиса мониторинга
    /// </summary>
    public class CheckSiteService : ICheckSiteService
    {
        protected IConfiguration Configuration;
        protected ILogger<CheckSiteService> Logger;

        public CheckSiteService(ILogger<CheckSiteService> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <inheritdoc />
        public async Task<SiteDTO> AddOrUpdateJob(SiteDTO model)
        {
            if (model == null)
            {
                throw new ArgumentException($"Не передана объектная модель {nameof(model)}.");
            }

            var serviceApiUrl = Configuration.GetValue<string>("Hosts:SiteMonitoring.Service");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var requestParams = JsonConvert.SerializeObject(model);
                    var requestContent = new StringContent(requestParams, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(
                            $"{serviceApiUrl}/api/CheckSite/AddOrUpdateJob", requestContent);

                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync();
    
                    switch (response?.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        {
                            return JsonConvert.DeserializeObject<SiteDTO>(responseText);
                            }
                        default:
                        {
                            throw new HttpRequestException($"StatusCode:{response.StatusCode}. ErrorText:{responseText}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Logger.LogError("Произошла ошибка при обращении к сервису.", e);
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteJob(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentException("Не пердан идентификатор объекта.");
            }

            var serviceApiUrl = Configuration.GetValue<string>("Hosts:SiteMonitoring.Service");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.DeleteAsync(
                        $"{serviceApiUrl}/api/CheckSite/{jobId}");
                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync();

                    switch (response?.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        {
                            return;
                        }
                        default:
                        {
                            throw new Exception($"StatusCode:{response.StatusCode}. ErrorText:{responseText}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Logger.LogError("Произошла ошибка при обращении к сервису.", e);
                    throw;
                }
            }
        }
    }
}

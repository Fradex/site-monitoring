using System;
using System.Net;
using Microsoft.Extensions.Logging;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Service.Jobs.Interfaces;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Service.Jobs
{
    /// <summary>
    /// Джоба для проверки сайта
    /// </summary>
    public class CheckSiteJob : ICheckSiteJob
    {
        protected ILogger<CheckSiteJob> Logger;
        protected ISiteService SiteService;

        public CheckSiteJob(ILogger<CheckSiteJob> logger, ISiteService siteService)
        {
            Logger = logger;
            SiteService = siteService;
        }
        /// <summary>
        /// Проверить сайт
        /// </summary>
        public bool CheckSite(SiteDTO site, bool isSendUpdate = true)
        {
            try
            {
                if (site == null)
                {
                    throw new ArgumentException($"Не передана объектная модель {nameof(site)}.");
                }

                if (string.IsNullOrWhiteSpace(site.Url))
                {
                    throw new ArgumentException($"Не передан URL.");
                }

                var request = WebRequest.Create(site.Url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    site.IsAvailable = response.StatusCode == HttpStatusCode.OK;
                    site.LastUpdatedDate = DateTime.Now;

                    if (isSendUpdate)
                    {
                        SiteService.UpdateSiteStatus(site);
                    }
                    return site.IsAvailable;
                }
            }
            catch (Exception exception)
            {
                this.Logger.LogError("Что-то пошло не так.", exception);
            }

            return false;
        }
    }
}

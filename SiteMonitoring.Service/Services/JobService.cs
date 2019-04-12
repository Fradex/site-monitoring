using System;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Service.Jobs.Interfaces;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Service.Services
{
    /// <summary>
    /// Сервис для периодических задач.
    /// </summary>
    public class JobService : IJobService
    {
        protected IConfiguration Configuration;
        protected ISiteService SiteService;
        protected ILogger<JobService> Logger;
        protected IServiceProvider ServiceProvider;

        public JobService(ILogger<JobService> logger, IConfiguration configuration, ISiteService siteService, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            Logger = logger;
            SiteService = siteService;
            ServiceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public void RunAllJobs()
        {
            try
            {
                var sites = this.SiteService.GetAllSites().Result;

                foreach (var site in sites)
                {
                    this.AddOrUpdateJobInternal(site);
                }
            }
            catch (Exception e)
            {
                //Если упало, то просто пишем в лог
                Logger.LogError("Что-то пошло не так, и все упало.", e);

                //Запустим еще раз через 5 минут
                BackgroundJob.Schedule(
                    () => RunAllJobs(),
                    TimeSpan.FromMinutes(5));
            }
        }

        /// <inheritdoc />
        public void DeleteJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        /// <inheritdoc />
        public SiteDTO AddOrUpdateJob(SiteDTO site)
        {
            return this.AddOrUpdateJobInternal(site);
        }

        private SiteDTO AddOrUpdateJobInternal(SiteDTO site)
        {
            if (string.IsNullOrEmpty(site.JobId))
            {
                site.JobId = $"CheckSiteJob-{Guid.NewGuid()}";
            }

            var service = ServiceProvider.GetService<ICheckSiteJob>();

            RecurringJob.AddOrUpdate(site.JobId, () => service.CheckSite(site, true), Cron.MinuteInterval(site.CheckedInterval));
            site.IsAvailable = service.CheckSite(site, false);
            return site;
        }
    }
}

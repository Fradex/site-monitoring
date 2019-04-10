using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Service.Jobs;
using SiteMonitoring.Service.Jobs.Interfaces;
using SiteMonitoring.Service.Services.Interfaces;

namespace SiteMonitoring.Test.SiteMonitoringService
{
    [TestClass]
    public class CheckSiteJobTest
    {
        protected ICheckSiteJob CheckSiteJob;

        [TestInitialize]
        public void Init()
        {
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var logger = loggerFactory.CreateLogger<CheckSiteJob>();
            var siteService = Substitute.For<ISiteService>();
            siteService.UpdateSiteStatus(Arg.Any<SiteDTO>())
                .Returns(Task.Delay(1000));

            CheckSiteJob = new CheckSiteJob(logger, siteService);
        }

        [TestMethod]
        public void CheckSite()
        {
            var obj = new SiteDTO
            {
                Url = "https://google.com"
            };
            var result = CheckSiteJob.CheckSite(obj, false);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckNotUrl()
        {
            var obj = new SiteDTO
            {
                Url = null
            };
            var result = CheckSiteJob.CheckSite(obj, false);

            Assert.IsFalse(result);
        }
    }
}

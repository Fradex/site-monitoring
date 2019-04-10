using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SiteMonitoring.Context;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Repositories;
using SiteMonitoring.Repositories.Interfaces;
using SiteMonitoring.Services.Interfaces;

namespace SiteMonitoring.Test.SiteMonitoring
{
    [TestClass]
    public class SiteRepositoryTests
    {
        protected ISiteRepository SiteRepository;
        protected SiteContext Context;
        protected SiteDTO Site;

        [TestInitialize]
        public void Init()
        {
            this.Site = new SiteDTO()
            {
                CheckedInterval = 5,
                Description = "Test",
                IsAvailable = false,
                JobId = Guid.NewGuid().ToString(),
                LastUpdatedDate = DateTime.MaxValue,
                Name = "Test",
                Url = "http://test.tu"
            };
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "testDB")
                .Options;

            Context = new SiteContext(options);
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SiteRepository>();

            var checkService = Substitute.For<ICheckSiteService>();
            checkService.AddOrUpdateJob(Arg.Any<SiteDTO>())
                .Returns(Site);

            this.SiteRepository = new SiteRepository(logger, Context, checkService);
        }

        [TestMethod]
        public async Task CheckSaveAsync()
        {
            var res = await SiteRepository.SaveAsync(Site);

            Assert.AreEqual(res.CheckedInterval, Site.CheckedInterval);
            Assert.IsFalse(res.IsAvailable);
            Assert.AreEqual(res.JobId, Site.JobId);
            Assert.AreEqual(res.LastUpdatedDate, DateTime.MaxValue);
            Assert.AreEqual(res.Description, Site.Description);
            Assert.AreEqual(res.Name, Site.Name);
            Assert.AreEqual(res.Url, Site.Url);
            Assert.IsNotNull(res.Id);
        }

        [TestMethod]
        public async Task CheckDeleteAsync()
        {
            var res = await SiteRepository.SaveAsync(Site);
            await SiteRepository.DeleteAsync(res.Id);
            var res1 = await this.SiteRepository.GetAsync(res.Id);

            Assert.IsNull(res1);
        }

        [TestMethod]
        public async Task CheckChangeStatusAsync()
        {
           var site1 = new SiteDTO()
            {
                CheckedInterval = 5,
                Description = "Test",
                IsAvailable = true,
                JobId = Guid.NewGuid().ToString(),
                LastUpdatedDate = DateTime.MaxValue,
                Name = "Test",
                Url = "http://test.tu"
            };

            var res = await SiteRepository.SaveAsync(site1);
            await SiteRepository.UpdateStatus(res);
            var res1 = await SiteRepository.GetAsync(res.Id);

            Assert.AreNotEqual(site1.IsAvailable , res1.IsAvailable);
        }

        [TestCleanup]
        public void Destroy()
        {
            Context?.Dispose();
        }
    }
}

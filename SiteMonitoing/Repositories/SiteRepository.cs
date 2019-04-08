using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiteMonitoring.Context;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Model.Model.Requests;
using SiteMonitoring.Repositories.Interfaces;

namespace SiteMonitoring.Repositories
{
    /// <summary>
    /// Репозиторий для работы с сущностью Сайт
    /// </summary>
    public class SiteRepository : ISiteRepository
    {
        protected ILogger<SiteRepository> Logger;
        protected SiteContext Context;

        public SiteRepository(ILogger<SiteRepository> logger, SiteContext context)
        {
            Logger = logger;
            Context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SiteDTO>> GetAllAsync()
        {
            return await Context.SiteStatuses.Select(x => new SiteDTO
            {
                Id = x.Site.Id,
                Name = x.Site.Name,
                Url = x.Site.Url,
                Description = x.Site.Description,
                CheckedInterval = x.CheckedInterval,
                IsAvailable = x.IsAvailable
            }).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<SiteDTO> GetAsync(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("Не передан идентификатор объекта.");
            }

            return await Context.SiteStatuses
                .Where(x => x.Site.Id == id)
                .Select(x => new SiteDTO
                {
                    Id = x.Site.Id,
                    Name = x.Site.Name,
                    Url = x.Site.Url,
                    Description = x.Site.Description,
                    CheckedInterval = x.CheckedInterval,
                    IsAvailable = x.IsAvailable
                }).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task SaveAsync(SiteRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentException("Передана пустая объектная модель.");
            }

            var site = await Context.Sites.Select(x => x)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (site != null)
            {
                var siteEntry = Context.Entry(site);
                siteEntry.CurrentValues.SetValues(model);
                siteEntry.State = EntityState.Modified;

                var siteStatus = await Context.SiteStatuses.Select(x => x)
                    .FirstOrDefaultAsync(x => x.Site.Id == model.Id);
                var siteStatusEntry = Context.Entry(siteStatus);
                siteStatusEntry.Entity.CheckedInterval = model.CheckedInterval;
                siteStatusEntry.State = EntityState.Modified;
            }
            else
            {
                var siteEntry = await Context.Sites.AddAsync(model.ToSite());

                if (siteEntry != null)
                {
                    var siteStatus = new SiteStatus
                    {
                        Site = siteEntry.Entity,
                        CheckedInterval = model.CheckedInterval == 0 ? 5 : model.CheckedInterval
                    };
                    Context.SiteStatuses.Add(siteStatus);
                }
                model.Id = siteEntry.Entity.Id;
            }

            await Context.SaveChangesAsync();
        }
    }
}

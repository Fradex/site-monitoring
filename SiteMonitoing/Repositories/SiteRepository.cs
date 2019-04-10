using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SiteMonitoring.Context;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Repositories.Interfaces;
using SiteMonitoring.Services.Interfaces;

namespace SiteMonitoring.Repositories
{
    /// <summary>
    /// Репозиторий для работы с сущностью Сайт
    /// </summary>
    public class SiteRepository : ISiteRepository
    {
        protected ILogger<SiteRepository> Logger;
        protected SiteContext Context;
        protected ICheckSiteService CheckSiteService;

        public SiteRepository(ILogger<SiteRepository> logger, SiteContext context, ICheckSiteService checkSiteService)
        {
            Logger = logger;
            Context = context;
            this.CheckSiteService = checkSiteService;
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
                IsAvailable = x.IsAvailable,
                LastUpdatedDate = x.LastUpdatedDate
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
                    IsAvailable = x.IsAvailable,
                    JobId = x.JobId,
                    LastUpdatedDate = x.LastUpdatedDate
                }).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<SiteDTO> SaveAsync(SiteDTO model)
        {
            if (model == null)
            {
                throw new ArgumentException("Передана пустая объектная модель.");
            }

            EntityEntry<SiteStatus> siteStatusEntry = null;
            Guid id;
            var site = await Context.Sites.Select(x => x)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (site != null)
            {
                var siteEntry = Context.Entry(site);
                siteEntry.CurrentValues.SetValues(model);
                siteEntry.State = EntityState.Modified;

                var siteStatus = await Context.SiteStatuses.Select(x => x)
                     .FirstOrDefaultAsync(x => x.Site.Id == model.Id);
                siteStatusEntry = Context.Entry(siteStatus);
                siteStatusEntry.Entity.CheckedInterval = model.CheckedInterval;
                siteStatusEntry.State = EntityState.Modified;
                id = site.Id;
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
                    siteStatusEntry = await Context.SiteStatuses.AddAsync(siteStatus);
                }
                id = siteEntry.Entity.Id;
            }

            if (siteStatusEntry != null)
            {
                await this.UpdateStatusFields(model, siteStatusEntry);
            }

            await Context.SaveChangesAsync();
            return await GetAsync(id);
        }

        private async Task UpdateStatusFields(SiteDTO requestModel, EntityEntry<SiteStatus> siteStatusEntry)
        {
            //Выполняем проверку на сервисе и обновляем поля
            try
            {
                var response = await CheckSiteService.AddOrUpdateJob(requestModel);
                siteStatusEntry.Entity.JobId = response.JobId;
                siteStatusEntry.Entity.LastUpdatedDate = response.LastUpdatedDate;
                siteStatusEntry.Entity.IsAvailable = response.IsAvailable;
            }
            catch (Exception e)
            {
                this.Logger.LogError("Сервис не доступен.", e);
            }
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException("Не передан идентификатор объекта.");
            }

            var site = await Context.Sites.Select(x => x)
                .FirstOrDefaultAsync(x => x.Id == id);
            var siteStatus = await Context.SiteStatuses.Select(x => x)
                .FirstOrDefaultAsync(x => x.Site.Id == id);

            if (site != null && siteStatus != null)
            {
                this.Context.SiteStatuses.Remove(siteStatus);
                this.Context.Sites.Remove(site);

                if (siteStatus.JobId != null)
                {
                    //Удаляем джобу с сервиса
                    await this.CheckSiteService.DeleteJob(siteStatus.JobId);
                }
            }
            else
            {
                this.Logger.LogError($"Объект не найден, либо в БД некорректные записи. ИД объекта {id}.");
            }

            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateStatus(SiteDTO model)
        {
            if (model == null)
            {
                throw new ArgumentException("Передана пустая объектная модель.");
            }

            var site = await Context.Sites.Select(x => x)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (site != null)
            {
                var siteStatus = await Context.SiteStatuses.Select(x => x)
                    .FirstOrDefaultAsync(x => x.Site.Id == model.Id);

                var siteStatusEntry = Context.Entry(siteStatus);

                siteStatusEntry.Entity.IsAvailable = model.IsAvailable;
                siteStatusEntry.Entity.JobId = model.JobId;
                siteStatusEntry.Entity.LastUpdatedDate = model.LastUpdatedDate;
                siteStatusEntry.State = EntityState.Modified;

                await Context.SaveChangesAsync();
            }
        }
    }
}

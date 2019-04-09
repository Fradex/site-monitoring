using System.Threading.Tasks;
using SiteMonitoring.Model.Model;

namespace SiteMonitoring.Services.Interfaces
{
    public interface ICheckSiteService
    {
        /// <summary>
        /// Обновить задачу сервиса мониторинга
        /// </summary>
        Task<SiteDTO> AddOrUpdateJob(SiteDTO site);

        /// <summary>
        /// Удалить задачу на сервисе мониторинга
        /// </summary>
        Task DeleteJob(string jobId);
    }
}

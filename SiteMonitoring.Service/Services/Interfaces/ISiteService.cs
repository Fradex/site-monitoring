using System.Collections.Generic;
using System.Threading.Tasks;
using SiteMonitoring.Model.Model;

namespace SiteMonitoring.Service.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с объектами сайта
    /// </summary>
    public interface ISiteService
    {
        /// <summary>
        /// Получить все записи объекта сайт
        /// </summary>
        Task<List<SiteDTO>> GetAllSites();

        /// <summary>
        /// Обновить статус записи объекта сайт
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateSiteStatus(SiteDTO model);
    }
}

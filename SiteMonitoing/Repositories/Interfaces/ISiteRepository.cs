using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteMonitoring.Model.Model;
using SiteMonitoring.Model.Model.Requests;

namespace SiteMonitoring.Repositories.Interfaces
{
    public interface ISiteRepository
    {
        /// <summary>
        /// Получить все записи без ограничений. !!! Опасная операция на больших объемах данных (Здесь должна быть ваша пагинация)
        /// </summary>
        /// <returns>перечисление дто-шек с сайтами</returns>
        Task<IEnumerable<SiteDTO>> GetAllAsync();

        /// <summary>
        /// Получить запись по id
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Дто сайта</returns>
        Task<SiteDTO> GetAsync(Guid id);

        /// <summary>
        /// Сохранить объект сайта
        /// </summary>
        /// <param name="model">Объектная модель сайта</param>
        /// <returns>таска</returns>
        Task SaveAsync(SiteRequestModel model);
    }
}

using System;

namespace SiteMonitoring.Model.Model
{
    /// <summary>
    /// DTO-шка для сайта
    /// </summary>
    public class SiteDTO: Site
    {
        /// <summary>
        /// Интервал для проверки
        /// </summary>
        public int CheckedInterval { get; set; }

        /// <summary>
        /// Сайт доступен
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        public Site ToSite()
        {
            return new Site
            {
                Description = this.Description,
                Name = this.Name,
                Url = this.Url
            };
        }
    }
}

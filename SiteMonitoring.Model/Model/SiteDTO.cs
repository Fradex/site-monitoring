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
    }
}

using System;

namespace SiteMonitoring.Model.Model
{
    /// <summary>
    /// Сайт для мониториинга
    /// </summary>
    public class Site
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Наименование 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace SiteMonitoring.Model.Model
{
    /// <summary>
    /// Класс для конфигурации и проверки доступности 
    /// </summary>
    public class SiteStatus
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ссылка на сайт
        /// </summary>
        [Required]
        public Site Site { get; set; }

        /// <summary>
        /// Интервал для проверки - дефолтный 5 минут
        /// </summary>
        public int CheckedInterval { get; set; } = 5;

        /// <summary>
        /// Сайт доступен
        /// </summary>
        public bool IsAvailable { get; set; } = false;

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}

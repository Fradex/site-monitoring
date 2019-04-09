using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMonitoring.Model.Model.Requests
{
    /// <summary>
    /// Реквест для обновления джобы
    /// </summary>
    public class JobRequest
    {
        /// <summary>
        /// ИД задачи
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Интервал
        /// </summary>
        public int Interval { get; set; }
    }
}

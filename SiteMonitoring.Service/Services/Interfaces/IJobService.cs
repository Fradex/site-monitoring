using SiteMonitoring.Model.Model;

namespace SiteMonitoring.Service.Services.Interfaces
{
    public interface IJobService
    {
        /// <summary>
        /// Запусить задачи для проверки доступности сайтов
        /// </summary>
        void RunAllJobs();

        /// <summary>
        /// Удалить джобу по ИД
        /// </summary>
        /// <param name="jobId">ИД</param>
        void DeleteJob(string jobId);

        /// <summary>
        /// Обновить джобу
        /// </summary>
        /// <param name="siteDto">Модель сайта</param>
        SiteDTO AddOrUpdateJob(SiteDTO siteDto);
    }
}

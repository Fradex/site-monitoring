using SiteMonitoring.Model.Model;

namespace SiteMonitoring.Service.Jobs.Interfaces
{
    public interface ICheckSiteJob
    {
        /// <summary>
        /// Проверить сайт
        /// </summary>
        bool CheckSite(SiteDTO site, bool isSendUpdate = true);
    }
}

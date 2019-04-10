using Hangfire.Dashboard;

namespace SiteMonitoring.Service.Authorization
{
    /// <summary>
    /// Фильтр авторизации
    /// Хенгфайр не хочет работать под докером без прав (
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}

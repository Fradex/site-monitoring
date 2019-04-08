namespace SiteMonitoring.Model.Model.Requests
{
    /// <summary>
    /// Реквест модель для сущности Сайт.
    /// </summary>
    public class SiteRequestModel : SiteDTO
    {
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

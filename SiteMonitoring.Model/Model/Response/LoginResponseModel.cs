namespace SiteMonitoring.Model.Model.Response
{
    public class LoginResponseModel
    {
        /// <summary>
        /// токен авторизации
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Успешна ли авторизация
        /// </summary>
        public bool Success { get; set; }
    }
}

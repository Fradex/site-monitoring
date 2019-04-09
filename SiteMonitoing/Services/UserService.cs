using System;
using SiteMonitoring.Services.Interfaces;

namespace SiteMonitoring.Services
{
    /// <summary>
    /// Сервис фикция и работает только для 1 пользователя
    /// </summary>
    public class UserService : IUserService
    {
        private const string AdminLogin = "admin";
        private const string AdminPassword = "admin";

        /// <inheritdoc />
        public string GetUserToken(string login, string password)
        {
            if (AdminLogin != login || AdminPassword != password)
            {
                throw new Exception("Пользователь не найден в системе.");
            }
            //Здесь должно быть получение пользователя, но его нет.
            return JWTService.GenerateUserToken(login, password, Guid.NewGuid().ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitoring.Services.Interfaces
{
    /// <summary>
    /// Фикция для эмуляции пользовательского сервиса.
    /// </summary>
    public interface IUserService
    {
        string GetUserToken(string login, string password);
    }
}

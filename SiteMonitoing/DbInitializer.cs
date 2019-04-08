using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitoring.Context;

namespace SiteMonitoring
{
    /// <summary>
    /// Класс расширения для стартапа
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Произвести миграцию БД
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public static void InitMigrations(this IServiceProvider applicationBuilder)
        {
            using (var context = applicationBuilder.GetRequiredService<SiteContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}

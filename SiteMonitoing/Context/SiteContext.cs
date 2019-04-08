using Microsoft.EntityFrameworkCore;
using SiteMonitoring.Model.Model;

namespace SiteMonitoring.Context
{
    /// <summary>
    /// Контекст для работы с EF
    /// </summary>
    public class SiteContext : DbContext
    {
        public SiteContext()
        {
        }

        public SiteContext(DbContextOptions<SiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SiteStatus> SiteStatuses { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

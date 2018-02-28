using KpdApps.Orationi.Messaging.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.DataAccess
{
    public class OrationiMessagingContext : DbContext
    {
        public OrationiMessagingContext(DbContextOptions<OrationiMessagingContext> options)
            : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RequestCode> RequestCodes { get; set; }

        public DbSet<RequestCodeAlias> RequestCodeAliases { get; set; }

        public DbSet<PluginAssembly> PluginAsseblies { get; set; }

        public DbSet<PluginType> PluginTypes { get; set; }

        public DbSet<PluginRegisteredStep> PluginRegisteredSteps { get; set; }

        public DbSet<GlobalSetting> GlobalSettings { get; set; }

        public DbSet<ProcessingError> ProcessingErrors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<RequestCode>().ToTable("RequestCodes");
            modelBuilder.Entity<RequestCodeAlias>().ToTable("RequestCodeAliases");
            modelBuilder.Entity<PluginAssembly>().ToTable("PluginAssemblies");
            modelBuilder.Entity<PluginType>().ToTable("PluginTypes");
            modelBuilder.Entity<PluginRegisteredStep>().ToTable("PluginRegisteredSteps");
            modelBuilder.Entity<GlobalSetting>().ToTable("GlobalSettings");
            modelBuilder.Entity<ProcessingError>().ToTable("ProcessingErrors");
        }
    }
}

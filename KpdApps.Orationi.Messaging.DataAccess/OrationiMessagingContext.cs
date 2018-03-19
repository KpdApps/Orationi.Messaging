using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.DataAccess
{
    public class OrationiMessagingContext : DbContext
    {
        public OrationiMessagingContext(IContextOptionsBuilder optionsBuilder)
            : base(optionsBuilder.GetThroughSettings())
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

        public DbSet<ExternalSystem> ExternalSystems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Message>()
                .ToTable("Messages");
            
            modelBuilder
                .Entity<RequestCode>()
                .ToTable("RequestCodes")
                .Property(p => p.RequestCodeId).HasColumnName("Id");
            
            modelBuilder
                .Entity<RequestCodeAlias>()
                .ToTable("RequestCodeAliases");
            
            modelBuilder
                .Entity<PluginAssembly>()
                .ToTable("PluginAssemblies");
            
            modelBuilder
                .Entity<PluginType>()
                .ToTable("PluginTypes");
            
            modelBuilder
                .Entity<PluginRegisteredStep>()
                .ToTable("PluginRegisteredSteps");
            
            modelBuilder
                .Entity<GlobalSetting>()
                .ToTable("GlobalSettings");
            
            modelBuilder
                .Entity<ProcessingError>()
                .ToTable("ProcessingErrors");
            
            modelBuilder
                .Entity<ExternalSystem>()
                .ToTable("ExternalSystems")
                .Property(p => p.ExternalSystemId)
                .HasColumnName("Id");
            
            modelBuilder
                .Entity<ExternalSystemRequestCode>()
                .ToTable("ExternalSystemsRequestCodes")
                .HasKey(entity => new { entity.ExternalSystemId, entity.RequestCodeId });
            
            modelBuilder
                .Entity<ExternalSystemRequestCode>()
                .HasOne(entity => entity.ExternalSystem)
                .WithMany(entity => entity.EsternalsSystemRequestCodes)
                .HasForeignKey(entity => entity.ExternalSystemId);
            
            modelBuilder
                .Entity<ExternalSystemRequestCode>()
                .HasOne(entity => entity.RequestCode)
                .WithMany(entity => entity.EsternalsSystemRequestCodes)
                .HasForeignKey(entity => entity.RequestCodeId);
        }
    }
}

using KpdApps.Orationi.Messaging.DataAccess.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.DataAccess
{
    public class OrationiMessagingContext : DbContext
    {
        public OrationiMessagingContext(IContextOptionsBuilder optionsBuilder) : base(optionsBuilder.GetThroughSettings())
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RequestCode> RequestCodes { get; set; }

        public DbSet<RequestCodeAlias> RequestCodeAliases { get; set; }

        public DbSet<PluginAssembly> PluginAsseblies { get; set; }

        public DbSet<PluginActionSet> PluginActionSets { get; set; }

        public DbSet<PluginActionSetItem> PluginActionSetItems { get; set; }

        public DbSet<GlobalSetting> GlobalSettings { get; set; }

        public DbSet<ProcessingError> ProcessingErrors { get; set; }

        public DbSet<ExternalSystem> ExternalSystems { get; set; }

        public DbSet<Workflow> Workflows { get; set; }

        public DbSet<WorkflowAction> WorkflowActions { get; set; }

        public DbSet<RegisteredPlugin> RegisteredPlugins { get; set; }

        public DbSet<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }

        public DbSet<ExternalSystemRequestCode> ExternalSystemRequestCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Message>()
                .ToTable("Messages");

            modelBuilder
                .Entity<RequestCode>()
                .ToTable("RequestCodes");

            modelBuilder
                .Entity<RequestCodeAlias>()
                .ToTable("RequestCodeAliases");

            modelBuilder
                .Entity<PluginAssembly>()
                .ToTable("PluginAssemblies");

            modelBuilder
                .Entity<RegisteredPlugin>()
                .ToTable("RegisteredPlugins");

            modelBuilder
                .Entity<PluginActionSet>()
                .ToTable("PluginActionSets");

            modelBuilder
                .Entity<PluginActionSetItem>()
                .ToTable("PluginActionSetItems");

            modelBuilder
                .Entity<GlobalSetting>()
                .ToTable("GlobalSettings");

            modelBuilder
                .Entity<ProcessingError>()
                .ToTable("ProcessingErrors");

            modelBuilder
                .Entity<ExternalSystem>()
                .ToTable("ExternalSystems");

            modelBuilder
                .Entity<Workflow>()
                .ToTable("Workflows");

            modelBuilder
                .Entity<WorkflowAction>()
                .ToTable("WorkflowActions");

            modelBuilder
                .Entity<WorkflowExecutionStep>()
                .ToTable("WorkflowExecutionSteps");

            modelBuilder
                .Entity<ExternalSystemRequestCode>()
                .ToTable("ExternalSystemRequestCodes");
        }
    }
}

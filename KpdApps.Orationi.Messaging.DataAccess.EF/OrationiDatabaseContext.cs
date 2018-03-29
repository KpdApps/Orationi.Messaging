using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using KpdApps.Orationi.Messaging.DataAccess.EF.EntityConfigurations;
using KpdApps.Orationi.Messaging.DataAccess.EF.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EF
{
    public class OrationiDatabaseContext : DbContext
    {
        static OrationiDatabaseContext()
        {
            Database.SetInitializer(new NullDatabaseInitializer<OrationiDatabaseContext>());
        }

        public OrationiDatabaseContext() : base("OrationiConnectionString")
        {

        }

        public DbSet<RequestCodeAlias> RequestCodeAliases { get; set; }

        public DbSet<ProcessingError> ProcessingErrors { get; set; }

        public DbSet<GlobalSetting> GlobalSettings { get; set; }

        public DbSet<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }

        public DbSet<Workflow> Workflows { get; set; }

        public DbSet<PluginActionSet> PluginActionSets { get; set; }

        public DbSet<PluginAssembly> PluginAsseblies { get; set; }

        public DbSet<WorkflowAction> WorkflowActions { get; set; }

        public DbSet<RegisteredPlugin> RegisteredPlugins { get; set; }

        public DbSet<PluginActionSetItem> PluginActionSetItems { get; set; }

        public DbSet<RequestCode> RequestCodes { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<ExternalSystem> ExternalSystems { get; set; }

        public DbSet<MessageStatusCode> MessageStatusCodes { get; set; }

        public DbSet<WorkflowExecutionStepsStatusCode> WorkflowExecutionStepsStatusCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ExternalSystemTypeConfiguration());
            modelBuilder.Configurations.Add(new MessageTypeConfiguration());

            modelBuilder
                .Entity<RequestCodeAlias>()
                .ToTable("RequestCodeAliases");

            modelBuilder
                .Entity<ProcessingError>()
                .ToTable("ProcessingErrors")
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder
                .Entity<GlobalSetting>()
                .ToTable("GlobalSettings")
                .HasKey(key => key.Name);

            modelBuilder
                .Entity<WorkflowExecutionStep>()
                .ToTable("WorkflowExecutionSteps")
                .HasRequired(p => p.WorkflowExecutionStepsStatusCode)
                .WithMany(p => p.WorkflowExecutionSteps)
                .HasForeignKey(p => p.StatusCode);

            modelBuilder
                .Entity<Workflow>()
                .ToTable("Workflows");

            modelBuilder
                .Entity<PluginActionSet>()
                .ToTable("PluginActionSets");

            modelBuilder
                .Entity<PluginAssembly>()
                .ToTable("PluginAssemblies");

            modelBuilder
                .Entity<WorkflowAction>()
                .ToTable("WorkflowActions");

            modelBuilder
                .Entity<RegisteredPlugin>()
                .ToTable("RegisteredPlugins");

            modelBuilder
                .Entity<PluginActionSetItem>()
                .ToTable("PluginActionSetItems");

            modelBuilder
                .Entity<RequestCode>()
                .ToTable("RequestCodes");

            modelBuilder
                .Entity<MessageStatusCode>()
                .ToTable("MessageStatusCode");

            modelBuilder
                .Entity<WorkflowExecutionStepsStatusCode>()
                .ToTable("WorkflowExecutionStepsStatusCodes");
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess
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
            modelBuilder.Configurations.Add(new GlobalSettingTypeConfiguration());
            modelBuilder.Configurations.Add(new MessageTypeConfiguration());
            modelBuilder.Configurations.Add(new MessageStatusCodeEntityConfiguration());
            modelBuilder.Configurations.Add(new PluginActionSetTypeConfiguration());
            modelBuilder.Configurations.Add(new PluginActionSetItemTypeConfiguration());
            modelBuilder.Configurations.Add(new PluginAssemblyTypeConfiguration());
            modelBuilder.Configurations.Add(new ProcessingErrorTypeConfiguration());
            modelBuilder.Configurations.Add(new RegisteredPluginTypeConfiguration());
            modelBuilder.Configurations.Add(new RequestCodeTypeConfiguration());
            modelBuilder.Configurations.Add(new RequestCodeAliasTypeConfiguration());
            modelBuilder.Configurations.Add(new WorkflowTypeConfiguration());
            modelBuilder.Configurations.Add(new WorkflowActionTypeConfiguration());
            modelBuilder.Configurations.Add(new WorkflowExecutionStepTypeConfiguration());
            modelBuilder.Configurations.Add(new WorkflowExecutionStepsStatusCodeTypeConfiguration());
        }

        private void AddConfiguration<TConfiguration, TEntity>(DbModelBuilder modelBuilder)
            where TConfiguration : EntityTypeConfiguration<TEntity>, new ()
            where TEntity : class
        {
            modelBuilder.Configurations.Add(new TConfiguration());
        }
    }
}

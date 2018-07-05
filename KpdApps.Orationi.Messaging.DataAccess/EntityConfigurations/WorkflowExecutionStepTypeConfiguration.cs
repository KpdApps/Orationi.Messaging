using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class WorkflowExecutionStepTypeConfiguration : EntityTypeConfiguration<WorkflowExecutionStep>
    {
        public WorkflowExecutionStepTypeConfiguration()
        {
            ToTable("WorkflowExecutionSteps")
                .HasRequired(p => p.WorkflowExecutionStepsStatusCode)
                .WithMany(p => p.WorkflowExecutionSteps)
                .HasForeignKey(p => p.StatusCode);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Created)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}

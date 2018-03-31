using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class ProcessingErrorTypeConfiguration : EntityTypeConfiguration<ProcessingError>
    {
        public ProcessingErrorTypeConfiguration()
        {
            ToTable("ProcessingErrors")
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

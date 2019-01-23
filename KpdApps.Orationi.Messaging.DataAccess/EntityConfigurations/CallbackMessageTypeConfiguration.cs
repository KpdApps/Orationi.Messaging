using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class CallbackMessageTypeConfiguration : EntityTypeConfiguration<CallbackMessage>
    {
        public CallbackMessageTypeConfiguration()
        {
            ToTable("CallbackMessages");
            
            HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Modified)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}

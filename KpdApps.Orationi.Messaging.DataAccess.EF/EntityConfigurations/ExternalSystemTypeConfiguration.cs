using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.EF.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.EntityConfigurations
{
    public class ExternalSystemTypeConfiguration : EntityTypeConfiguration<ExternalSystem>
    {
        public ExternalSystemTypeConfiguration()
        {
            ToTable("ExternalSystems")
                .HasMany(p => p.RequestCodes)
                .WithMany(p => p.ExternalSystems)
                .Map(t => t.ToTable("ExternalSystemsRequestCodes")
                    .MapLeftKey("ExternalSystemId")
                    .MapRightKey("RequestCodeId"));

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

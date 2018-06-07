using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    class CacheRequestResponseTypeConfiguration : EntityTypeConfiguration<CacheRequestResponse>
    {
        public CacheRequestResponseTypeConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

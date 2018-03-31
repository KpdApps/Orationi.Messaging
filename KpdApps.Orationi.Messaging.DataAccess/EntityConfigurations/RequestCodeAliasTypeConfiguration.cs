using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    class RequestCodeAliasTypeConfiguration : EntityTypeConfiguration<RequestCodeAlias>
    {
        public RequestCodeAliasTypeConfiguration()
        {
            ToTable("RequestCodeAliases")
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

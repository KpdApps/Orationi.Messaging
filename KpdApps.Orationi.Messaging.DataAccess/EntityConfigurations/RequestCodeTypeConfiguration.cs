using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class RequestCodeTypeConfiguration : EntityTypeConfiguration<RequestCode>
    {
        public RequestCodeTypeConfiguration()
        {
            ToTable("RequestCodes");

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class RequestCodeTypeConfiguration : EntityTypeConfiguration<RequestCode>
    {
        public RequestCodeTypeConfiguration()
        {
            ToTable("RequestCodes");
        }
    }
}

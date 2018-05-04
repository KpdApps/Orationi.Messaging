using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class MessageStatusCodeEntityConfiguration : EntityTypeConfiguration<MessageStatusCode>
    {
        public MessageStatusCodeEntityConfiguration()
        {
            ToTable("MessageStatusCode");
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class RegisteredPluginTypeConfiguration : EntityTypeConfiguration<RegisteredPlugin>
    {
        public RegisteredPluginTypeConfiguration()
        {
            ToTable("RegisteredPlugins")
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class PluginAssemblyTypeConfiguration : EntityTypeConfiguration<PluginAssembly>
    {
        public PluginAssemblyTypeConfiguration()
        {
            ToTable("PluginAssemblies")
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

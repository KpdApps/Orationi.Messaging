using System.Data.Entity.ModelConfiguration;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
    public class GlobalSettingTypeConfiguration : EntityTypeConfiguration<GlobalSetting>
    {
        public GlobalSettingTypeConfiguration()
        {
            ToTable("GlobalSettings")
                .HasKey(key => key.Name);
        }
    }
}

using KpdApps.Orationi.Messaging.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.DataAccess.EntityConfigurations
{
	public class FileStoreTypeConfiguration : EntityTypeConfiguration<FileStore>
	{
		public FileStoreTypeConfiguration()
		{
			ToTable("FileStore");

			Property(p => p.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}
	}
}

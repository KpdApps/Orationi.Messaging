using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.DataAccess
{
	public static class OrationiMessagingContextExtension
	{
		public static DbContextOptions<OrationiMessagingContext> DefaultDbContextOptions()
		{
			var connectionBuilder = new SqlConnectionStringBuilder
			{
				ApplicationName = "KpdApps.Orationi.Messaging",	// для профилирования БД
				DataSource = @"hq-vm-tstsql.exiar.ru\insttst02",
				InitialCatalog = @"OrationiMessageBus",
				IntegratedSecurity = true,
				MultipleActiveResultSets = true
			};

			var optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
			optionsBuilder.UseSqlServer(connectionBuilder.ToString());
			return optionsBuilder.Options;
		}
	}
}

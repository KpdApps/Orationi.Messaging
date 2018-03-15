using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KpdApps.Orationi.Messaging.DataAccess.Services
{
    public class OrationiContextOptionsBuilder : IContextOptionsBuilder
    {
        private readonly IConfiguration _configuration;

        public OrationiContextOptionsBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Получение настроек контекста соединения с БД из файла конфигурации appsettings.json
        /// </summary>
        /// <returns>Настройки контекста соединения с БД</returns>
        public DbContextOptions<OrationiMessagingContext> GetThroughSettings()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            return optionsBuilder.Options;
        }

        /// <summary>
        /// Получение настроек контекста соединения с БД по умолчанию
        /// </summary>
        /// <returns>Настройки контекста соединения с БД</returns>
        public DbContextOptions<OrationiMessagingContext> GetDefault()
        {
            var connectionBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "KpdApps.Orationi.Messaging", // для профилирования БД
                DataSource = @"localhost",
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
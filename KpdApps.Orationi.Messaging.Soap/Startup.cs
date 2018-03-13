using System.ServiceModel;
using KpdApps.Orationi.Messaging.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoapCore;

namespace KpdApps.Orationi.Messaging
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrationiMessagingContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            var dbContext = new OrationiMessagingContext(optionsBuilder.Options);

            services.AddSingleton(new MessagingService(dbContext));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseSoapEndpoint<MessagingService>(path: "/api/soap/Messaging", binding: new BasicHttpBinding(), serializer: SoapSerializer.XmlSerializer);
        }
    }
}

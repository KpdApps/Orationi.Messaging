using System.ServiceModel;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoapCore;

namespace KpdApps.Orationi.Messaging
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContextOptionsBuilder, OrationiContextOptionsBuilder>();
            services.AddTransient<OrationiMessagingContext>();
            services.AddSingleton<MessagingService>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseSoapEndpoint<MessagingService>(path: "/api/soap/Messaging", binding: new BasicHttpBinding(), serializer: SoapSerializer.XmlSerializer);
        }
    }
}

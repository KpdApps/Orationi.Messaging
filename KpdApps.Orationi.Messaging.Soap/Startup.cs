using System.ServiceModel;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoapCore;

namespace KpdApps.Orationi.Messaging.Soap
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContextOptionsBuilder, OrationiContextOptionsBuilder>();
            services.AddTransient<OrationiMessagingContext>();
            services.AddTransient<MessagingService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
            app.UseSoapEndpoint<MessagingService>(path: "/api/soap/Messaging", binding: new BasicHttpBinding(), serializer: SoapSerializer.XmlSerializer);
        }
    }
}

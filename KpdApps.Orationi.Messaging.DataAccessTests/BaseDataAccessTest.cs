using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Services;
using KpdApps.Orationi.Messaging.ServerCore.Helpers;
using Microsoft.Extensions.Configuration;

namespace KpdApps.Orationi.Messaging.DataAccessTests
{
    public class BaseDataAccessTest
    {
        protected OrationiMessagingContext DbContext { get; set; }

        public BaseDataAccessTest()
        {
            DbContext = new OrationiMessagingContext(ContextOptionsBuilderExtensions.GetContextOptionsBuilder());
        }
    }
}

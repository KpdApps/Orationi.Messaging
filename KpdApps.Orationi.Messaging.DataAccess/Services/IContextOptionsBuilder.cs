using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.DataAccess.Services
{
    public interface IContextOptionsBuilder
    {
        DbContextOptions<OrationiMessagingContext> GetThroughSettings();

        DbContextOptions<OrationiMessagingContext> GetDefault();
    }
}
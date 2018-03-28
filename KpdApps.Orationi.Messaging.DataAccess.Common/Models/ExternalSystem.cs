using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class ExternalSystem
    {
        public Guid ExternalSystemId { get; set; }

        public string SystemName { get; set; }

        public string Token { get; set; }

        public virtual List<ExternalSystemRequestCode> EsternalsSystemRequestCodes { get; set; }
    }
}

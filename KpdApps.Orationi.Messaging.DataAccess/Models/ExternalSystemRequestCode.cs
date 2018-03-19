using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class ExternalSystemRequestCode
    {
        public Guid ExternalSystemId { get; set; }

        public ExternalSystem ExternalSystem { get; set; }

        public int RequestCodeId { get; set; }

        public RequestCode RequestCode { get; set; }
    }
}

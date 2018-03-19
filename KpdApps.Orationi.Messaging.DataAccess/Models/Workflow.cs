using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class Workflow
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int RequestCodeId { get; set; }

        public RequestCode RequestCode { get; set; }
    }
}

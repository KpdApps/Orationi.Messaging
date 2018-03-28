using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class ExternalSystem
    {
        public Guid Id { get; set; }

        public string SystemName { get; set; }

        public string Token { get; set; }

        public virtual List<RequestCode> RequestCodes { get; set; }

        public virtual List<Message> Messages { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.Core.Models
{
    public class RabbitRequest
    {
        public int RequestCode { get; set; }

        public Guid MessageId { get; set; }
    }
}

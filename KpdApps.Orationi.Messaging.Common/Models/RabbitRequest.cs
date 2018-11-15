using System;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    public class RabbitRequest
    {
        public int RequestCode { get; set; }

        public Guid MessageId { get; set; }

        public bool IsCallback { get; set; }
    }
}

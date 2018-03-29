using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Models
{
    public class ExternalSystem
    {
        public ExternalSystem()
        {
            RequestCodes = new List<RequestCode>();
            Messages = new List<Message>();
        }

        public Guid Id { get; set; }

        public string SystemName { get; set; }

        public string Token { get; set; }

        public virtual List<RequestCode> RequestCodes { get; set; }

        public virtual List<Message> Messages { get; set; }

    }
}

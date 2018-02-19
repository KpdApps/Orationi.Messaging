using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class RegisteredStep
    {
        public Guid Id { get; set; }

        public int RequestCode { get; set; }

        public int Order { get; set; }

        public bool IsAsynchronous { get; set; }
    }
}

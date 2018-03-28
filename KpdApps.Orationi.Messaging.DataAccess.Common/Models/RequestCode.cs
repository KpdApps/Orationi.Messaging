﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class RequestCode
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool NeedNotification { get; set; }

        public virtual List<Workflow> Workflows { get; set; }

        public virtual List<Message> Messages { get; set; }

        public virtual List<ExternalSystem> ExternalSystems { get; set; }
    }
}

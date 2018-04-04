using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class RequestCode
    {
        public RequestCode()
        {
            Workflows = new List<Workflow>();
            Messages = new List<Message>();
            ExternalSystems = new List<ExternalSystem>();
        }

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

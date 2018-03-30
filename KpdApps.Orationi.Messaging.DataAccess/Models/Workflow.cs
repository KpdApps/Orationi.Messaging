using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{ 
    public class Workflow
    {
        public Workflow()
        {
            WorkflowExecutionSteps = new List<WorkflowExecutionStep>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("RequestCode")]
        public int RequestCodeId { get; set; }

        public virtual RequestCode RequestCode { get; set; }

        public virtual List<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }
    }
}

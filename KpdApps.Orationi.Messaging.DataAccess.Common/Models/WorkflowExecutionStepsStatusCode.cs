using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class WorkflowExecutionStepsStatusCode
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<WorkflowExecutionStep> WorkflowExecutionSteps { get; set; }
    }
}

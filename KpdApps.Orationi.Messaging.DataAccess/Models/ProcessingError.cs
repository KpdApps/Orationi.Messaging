using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class ProcessingError
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class Workflow
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int RequestCodeId { get; set; }
    }
}

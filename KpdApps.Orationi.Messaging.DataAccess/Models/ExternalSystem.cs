using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class ExternalSystem
    {
        public int ExternalSystemId { get; set; }

        public string SystemName { get; set; }

        public string Token { get; set; }

        public virtual List<ExternalSystemRequestCode> EsternalsSystemRequestCodes { get; set; }
    }
}

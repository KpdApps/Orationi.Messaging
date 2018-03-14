using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class ExternalSystemRequestCode
    {
        public int ExternalSystemId { get; set; }

        public ExternalSystem ExternalSystem { get; set; }

        public int RequestCodeId { get; set; }

        public RequestCode RequestCode { get; set; }
    }
}

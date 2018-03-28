using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DataAccess.Common.Models
{
    public class RequestCode
    {
        public int RequestCodeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<ExternalSystemRequestCode> EsternalsSystemRequestCodes { get; set; }
    }
}

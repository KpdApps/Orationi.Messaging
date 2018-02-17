using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<DateTime> Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<DateTime> Modified { get; set; }

        public int RequestCode { get; set; }

        public string RequestBody { get; set; }

        public string RequestUser { get; set; }

        public string RequestSystem { get; set; }

        public string ResponseBody { get; set; }

        public string ResponseUser { get; set; }

        public string ResponseSystem { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StatusCode { get; set; }

        public Nullable<int> ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSyncRequest { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int AttemptCount { get; set; }
    }
}

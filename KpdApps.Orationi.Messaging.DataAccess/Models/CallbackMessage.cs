using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class CallbackMessage
    {
        public Guid Id { get; set; }

        [ForeignKey("Message")] public Guid MessageId { get; set; }

        public Nullable<DateTime> Modified { get; set; }

        public bool CanBeSend { get; set; }

        public bool WasSend { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public virtual Message Message { get; set; }
    }
}

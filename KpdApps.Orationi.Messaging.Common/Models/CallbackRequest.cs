using System;
using Newtonsoft.Json;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    public class CallbackRequest
    {
        public Guid MessageId { get; set; }

        public int Code { get; set; }

        public string Body { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

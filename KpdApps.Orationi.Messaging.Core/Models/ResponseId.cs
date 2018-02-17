using System;
using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Models
{
    [DataContract]
    public class ResponseId : ResponseBase
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}

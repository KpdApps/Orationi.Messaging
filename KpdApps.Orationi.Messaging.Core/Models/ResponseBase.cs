using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Models
{
    [DataContract]
    public abstract class ResponseBase
    {
        [DataMember]
        public bool IsError { get; set; }
        [DataMember]
        public string Error { get; set; }
    }
}

using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}

using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Models
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public int RequestCode { get; set; }

        [DataMember]
        public string RequestType { get; set; }

        [DataMember]
        public string RequestBody { get; set; }

        [DataMember]
        public string RequestSystemName { get; set; }

        [DataMember]
        public string RequestUserName { get; set; }

        [DataMember]
        public string RequestSecureKey { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Models
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public int RequestCode { set; get; }

        [DataMember]
        public string RequestType { set; get; }

        [DataMember]
        public string RequestBody { set; get; }

        [DataMember]
        public string RequestSystemName { set; get; }

        [DataMember]
        public string RequestUserName { set; get; }

        [DataMember]
        public string RequestSecureKey { set; get; }
    }
}

using System;
using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public class ResponseXsd : ResponseBase
    {
        [DataMember]
        public string RequestContractUri { get; set; }

        [DataMember]
        public string ResponseContractUri { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()},\r\n\"RequestContractUri\" : \"{RequestContractUri}\",\r\n\"ResponseContractUri\" : \"{ResponseContractUri}\"";
        }
    }
}

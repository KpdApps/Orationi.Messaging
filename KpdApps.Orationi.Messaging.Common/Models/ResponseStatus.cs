using System;
using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public class ResponseStatus : ResponseBase
    {
        [DataMember]
        public Nullable<int> StatusCode { get; set; }

        [DataMember]
        public string StatusName { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()},\r\n\"StatusCode\" : \"{StatusCode}\",\r\n\"StatusName\" : \"{StatusName}\"";
        }
    }
}

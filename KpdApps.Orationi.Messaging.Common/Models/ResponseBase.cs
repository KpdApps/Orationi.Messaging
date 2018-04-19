using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public abstract class ResponseBase
    {
        [DataMember]
        public bool IsError { get; set; }
        [DataMember]
        public string Error { get; set; }

        public override string ToString()
        {
            return $"\"IsError\" : \"{IsError}\",\r\n\"Error\" : \"{Error}\"";
        }
    }
}
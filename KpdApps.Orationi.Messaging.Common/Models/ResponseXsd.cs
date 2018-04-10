using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public class ResponseXsd : ResponseBase
    {
        [DataMember]
        public string RequestContract { get; set; }

        [DataMember]
        public string ResponseContract { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()},\r\n\"RequestContract\" : \"{RequestContract}\",\r\n\"ResponseContract\" : \"{ResponseContract}\"";
        }
    }
}

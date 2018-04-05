using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Common.Models
{
    [DataContract]
    public class Response : ResponseId
    {
        [DataMember]
        public string Body { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()},\r\n\"Body\" : \"{Body}\"";
        }
    }
}

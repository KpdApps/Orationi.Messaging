﻿using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging.Models
{
    [DataContract]
    public class Response : ResponseId
    {
        [DataMember]
        public string Body { get; set; }
    }
}

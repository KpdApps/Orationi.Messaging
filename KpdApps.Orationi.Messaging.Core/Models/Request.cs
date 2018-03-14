using System;
using System.Runtime.Serialization;

namespace KpdApps.Orationi.Messaging
{
    [DataContract]
    public class Request
    {
        [DataMember] public int Code { set; get; }

        [DataMember] public string Type { set; get; }

        [DataMember] public string Body { set; get; }

        [DataMember] public string UserName { set; get; }
    }
}

using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class CallbackSettings
    {
        public Guid Id { get; set; }

        public string MethodType { get; set; }

        public string UrlAccessUserName { get; set; }

        public string UrlAccessUserPassword { get; set; }

        public bool NeedAuthentification { get; set; }
    }
}

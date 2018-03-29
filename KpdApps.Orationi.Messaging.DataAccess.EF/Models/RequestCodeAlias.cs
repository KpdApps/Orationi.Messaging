using System;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Models
{
    public class RequestCodeAlias
    {
        public Guid Id { get; set; }
        public int RequestCode { get; set; }
        public string Alias { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class RequestCodeAlias
    {
        public Guid Id { get; set; }
        public int RequestCode { get; set; }
        public string Alias { get; set; }
    }
}

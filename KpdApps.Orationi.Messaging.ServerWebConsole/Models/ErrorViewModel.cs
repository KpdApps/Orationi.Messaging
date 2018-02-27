using System;

namespace KpdApps.Orationi.Messaging.ServerWebConsole.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
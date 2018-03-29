﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KpdApps.Orationi.Messaging.DataAccess.EF.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public Nullable<DateTime> Created { get; set; }

        public Nullable<DateTime> Modified { get; set; }

        [ForeignKey("RequestCode")]
        public int RequestCodeId { get; set; }

        [Column(TypeName = "xml not null")]
        public string RequestBody { get; set; }

        public string RequestUser { get; set; }

        [ForeignKey("ExternalSystem")]
        public Guid ExternalSystemId { get; set; }

        public string ResponseBody { get; set; }

        public string ResponseUser { get; set; }

        public string ResponseSystem { get; set; }

        public int StatusCode { get; set; }

        public Nullable<int> ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSyncRequest { get; set; }

        public int AttemptCount { get; set; }

        public virtual RequestCode RequestCode { get; set; }

        public virtual ExternalSystem ExternalSystem { get; set; }

        public virtual MessageStatusCode MessageStatusCode { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class CacheRequestResponse
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
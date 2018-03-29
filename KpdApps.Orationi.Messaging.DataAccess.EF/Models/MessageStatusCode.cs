﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Models
{
    public class MessageStatusCode
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}

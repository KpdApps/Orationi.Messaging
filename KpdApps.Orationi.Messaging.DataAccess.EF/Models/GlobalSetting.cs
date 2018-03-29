﻿using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Models
{
    public class GlobalSetting
    {
        [Key]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

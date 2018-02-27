using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class GlobalSetting
    {
        [Key]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

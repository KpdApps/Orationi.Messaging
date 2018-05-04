using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class MessageStatusCode
    {
	    public MessageStatusCode()
	    {
		    Messages = new List<Message>();
	    }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}

using System;

namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
	public class FileStore
	{
		public Guid Id { get; set; }

		public Guid MessageId { get; set; }

		public string FileName { get; set; }

		public byte[] Data { get; set; }

		public DateTime CreatedOn { get; set; }

		public virtual Message Message { get; set; }
	}
}
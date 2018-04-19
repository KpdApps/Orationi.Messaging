using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace KpdApps.Orationi.Messaging.Rest.Models
{
	public class UploadFileInfo
	{
		public Guid MessageId { get; set; }
		public string FileType { get; set; }

		public static void ValidateFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			if (fileName.Length > 250)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			string[] forbidenExtensions = { ".exe", ".dll" };

			var fileExtension = Path.GetExtension(fileName);

			if (forbidenExtensions.Contains(fileExtension.ToLower()))
				throw new HttpResponseException(HttpStatusCode.BadRequest);
		}
	}
}
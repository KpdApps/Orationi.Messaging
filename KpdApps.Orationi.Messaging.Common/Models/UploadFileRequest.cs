using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml.Linq;

namespace KpdApps.Orationi.Messaging.Common.Models
{
	public class UploadFileRequest
	{
		public Guid ObjectId { get; set; }

		public int ObjectCode { get; set; }

		public string FileType { get; set; }

		public int RequsetCode { get; set; }

		private const int MaxSharePointFileNameLegth = 250;

		public static void ValidateFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			if (fileName.Length > MaxSharePointFileNameLegth)
				throw new HttpResponseException(HttpStatusCode.BadRequest);

			string[] forbidenExtensions = { ".exe", ".dll" };

			var fileExtension = Path.GetExtension(fileName);

			if (forbidenExtensions.Contains(fileExtension.ToLower()))
				throw new HttpResponseException(HttpStatusCode.BadRequest);
		}

		public string ToXmlString(Guid messageId)
		{
			var xmlBody = new XElement("UploadFileRequest",
				new XElement("MessageId", messageId),
				new XElement("ObjectId", ObjectId),
				new XElement("ObjectCode", ObjectCode),
				new XElement("FileType", FileType)
			);

			return @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlBody.ToString(SaveOptions.DisableFormatting);
		}
	}
}
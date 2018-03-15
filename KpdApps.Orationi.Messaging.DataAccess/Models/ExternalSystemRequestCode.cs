namespace KpdApps.Orationi.Messaging.DataAccess.Models
{
    public class ExternalSystemRequestCode
    {
        public int ExternalSystemId { get; set; }

        public ExternalSystem ExternalSystem { get; set; }

        public int RequestCodeId { get; set; }

        public RequestCode RequestCode { get; set; }
    }
}

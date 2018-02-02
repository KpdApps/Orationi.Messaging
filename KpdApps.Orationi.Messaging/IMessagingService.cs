using System.ServiceModel;

namespace KpdApps.Orationi.Messaging
{
    [ServiceContract]
    interface IMessagingService
    {
        [OperationContract]
        string Ping(string msg);
    }
}

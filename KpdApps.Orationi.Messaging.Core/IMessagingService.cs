using System;
using System.ServiceModel;
using KpdApps.Orationi.Messaging.Common.Models;

namespace KpdApps.Orationi.Messaging.Core
{
    [ServiceContract]
    public interface IMessagingService
    {
        [OperationContract]
        string GetVersion();

        [OperationContract]
        Response GetResponse(Guid requestId);

        [OperationContract]
        Response GetStatus(Guid requestId);

        [OperationContract]
        Response ExecuteRequest(Request request);

        [OperationContract]
        ResponseId ExecuteRequestAsync(Request request);

        [OperationContract]
        ResponseId SendRequest(Request request);
    }
}

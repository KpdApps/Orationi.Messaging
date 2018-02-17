using KpdApps.Orationi.Messaging.Models;
using System;
using System.ServiceModel;

namespace KpdApps.Orationi.Messaging
{
    [ServiceContract]
    public interface IMessagingService
    {
        [OperationContract]
        Response GetResponse(Guid requestId);

        //[OperationContract]
        //ResponseStatus GetStatus(Guid requestId);

        [OperationContract]
        Response ExecuteRequest(Request request);

        [OperationContract]
        Response ExecuteRequestAsync(Request request);

        [OperationContract]
        ResponseId SendRequest(Request request);
    }
}

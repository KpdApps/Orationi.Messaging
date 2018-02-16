using KpdApps.Orationi.Messaging.Models;
using System;
using System.ServiceModel;

namespace KpdApps.Orationi.Messaging
{
    [ServiceContract]
    interface IMessagingService
    {
        [OperationContract]
        Response GetResponse(Guid requestId);

        //[OperationContract]
        //ResponseStatus GetStatus(Guid requestId);

        [OperationContract]
        Response ExecuteRequest(Request request);

        [OperationContract]
        ResponseId SendRequest(Request request);

        [OperationContract]
        ResponseId SendRequestAsync(Request request);
    }
}

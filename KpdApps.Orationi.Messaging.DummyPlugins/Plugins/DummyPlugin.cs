using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System;

namespace KpdApps.Orationi.Messaging.DummyPlugins
{
    public class DummyPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        const string _RequestContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyRequest.xsd";

        const string _ResponseContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyResponse.xsd";

        public DummyPlugin(IExecuteContext context)
            : base(context)
        {
            base.RequestContractUri = _RequestContractUri;
            base.ResponseContractUri = _ResponseContractUri;
        }

        public override void Execute()
        {
            DummyRequest request = DummyRequest.Deserialize(Context.RequestBody);

            DummyResponse response = new DummyResponse() { MessageId = request.MessageId, RequestCode = request.RequestCode, Status = "Done" };
            Context.ResponseBody = response.Serialize();
        }
    }
}

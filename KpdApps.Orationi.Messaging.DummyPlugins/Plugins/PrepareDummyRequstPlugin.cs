using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DummyPlugins
{
    public class PrepareDummyRequstPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        const string SystemName = "DummySystem";

        const string FakeUserName = "Dummy";

        const string SettingsKeySendNotification = "SendNotification";

        const string _RequestContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyRequest.xsd";

        const string _ResponseContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyResponse.xsd";

        public override string[] LocalSettingsList => base.LocalSettingsList;

        public PrepareDummyRequstPlugin(IPipelineExecutionContext context)
            : base(context)
        {
            base.RequestContractUri = _RequestContractUri;
            base.ResponseContractUri = _ResponseContractUri;
        }

        public override void Execute()
        {
            DummyRequest request = DummyRequest.Deserialize(Context.WorkflowExecutionContext.MessageBody);
            Context.RequestBody = request.Serialize();
            Context.PipelineValues.Add("DummyValue", 1);
        }

        public override void AfterExecution()
        {
            base.AfterExecution();
            Context.ResponseSystem = SystemName;
            Context.ResponseUser = FakeUserName;
        }
    }
}

using KpdApps.Orationi.Messaging.Sdk;
using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DummyPlugins
{
    public class PrepareDummyRequstPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        const string SettingsKeySendNotification = "SendNotification";

        const string _MessageContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyRequest.xsd";

        const string _RequestContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyRequest.xsd";

        const string _ResponseContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyResponse.xsd";

        public override string[] LocalSettingsList => base.LocalSettingsList;

        public PrepareDummyRequstPlugin(IPipelineExecutionContext context)
            : base(context)
        {
            base.MessageContractUri = _MessageContractUri;
            base.RequestContractUri = _RequestContractUri;
            base.ResponseContractUri = _ResponseContractUri;
        }

        public override void Execute()
        {
            XsdValidator.ValidateXml(Context.WorkflowExecutionContext.MessageBody, new[] { MessageContractUri }, typeof(DummyRequest));

            DummyRequest request = DummyRequest.Deserialize(Context.WorkflowExecutionContext.MessageBody);
            Context.RequestBody = request.Serialize();
            Context.PipelineValues.Add("DummyValue", 1);
        }
    }
}

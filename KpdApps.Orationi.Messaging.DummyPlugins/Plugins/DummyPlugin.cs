using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System;
using System.Collections.Generic;

namespace KpdApps.Orationi.Messaging.DummyPlugins
{
    public class DummyPlugin : BasePipelinePlugin, IPipelinePlugin
    {
        const string SystemName = "DummySystem";

        const string FakeUserName = "Dummy";

        const string SettingsKeySendNotification = "SendNotification";

        const string _RequestContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyRequest.xsd";

        const string _ResponseContractUri = "KpdApps.Orationi.Messaging.DummyPlugins.Contracts.Dummy.DummyResponse.xsd";

        public override string[] LocalSettingsList => base.LocalSettingsList;

        public DummyPlugin(IExecuteContext context)
            : base(context)
        {
            base.RequestContractUri = _RequestContractUri;
            base.ResponseContractUri = _ResponseContractUri;
        }

        public override void Execute()
        {
            DummyRequest request = DummyRequest.Deserialize(Context.RequestBody);

            Context.PipelineValues.Add("TelegramMessages", new List<string>() { "Вы получили данное сообщение потому что тестировщик наркоман." });

            if (Context.PipelineValues.Contains("TelegramMessages"))
            {
                var telegramMessages = Context.PipelineValues["TelegramMessages"] as List<string>;
                telegramMessages.Add("Еще одно сообщение");
            }

            DummyResponse response = new DummyResponse() { MessageId = request.MessageId, RequestCode = request.RequestCode, Status = "Done" };
            Context.ResponseBody = response.Serialize();
        }

        public override void AfterExecution()
        {
            base.AfterExecution();
            Context.ResponseSystem = SystemName;
            Context.ResponseUser = FakeUserName;
        }
    }
}

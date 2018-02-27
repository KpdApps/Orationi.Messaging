using KpdApps.Orationi.Messaging.Sdk.Plugins;
using System.Threading.Tasks;

namespace KpdApps.Orationi.Messaging.TelegramPlugins
{
    public class TelegramMessagePlugin : IPipelinePlugin
    {
        const string TelegramBotApiToken = "TelegramBot.ApiToken";

        const string SettingsKeyChatId = "ChatId";

        public TelegramMessagePlugin(IExecuteContext context)
        {
            _context = context;
        }

        private IExecuteContext _context;
        public IExecuteContext Context => _context;

        public string RequestContractUri => string.Empty;

        public string ResponseContractUri => string.Empty;

        public void BeforeExecution()
        {

        }

        public void Execute()
        {
            if (!_context.PluginStepSettings.Contains(SettingsKeyChatId))
            {
                return;
            }

            long chatId = (long)_context.PluginStepSettings[SettingsKeyChatId];

            Telegram.Bot.TelegramBotClient client = new Telegram.Bot.TelegramBotClient(_context.GlobalSettings[TelegramBotApiToken].ToString());
            Task.Run(async () =>
            {
                Telegram.Bot.Types.Message message = await client.SendTextMessageAsync(chatId, "This is test message from RabbitMQ");
            });
        }

        public void AfterExecution()
        {

        }
    }
}

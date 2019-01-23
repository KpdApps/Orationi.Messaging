using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using log4net;
using log4net.Config;
using RestSharp;

namespace KpdApps.Orationi.Messaging.ServerCore.Callback
{
    public class CallbackHostManager : IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly double _checkFrequency;
        private bool _isRunning = false;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CallbackHostManager));

        public CallbackHostManager(double checkFrequency)
        {
            XmlConfigurator.Configure();
            _checkFrequency = checkFrequency;
            if (_checkFrequency < 0 || _checkFrequency > 3600)
            {
                throw new InvalidOperationException(
                    "Частота проверки callback-сообщений для их последующей отправки должна быть задана"
                    + " в диапазоне [0, 3600] секунд");
            }
        }

        public void Start()
        {
            Task.Run(() => Run(_cts.Token));
            _isRunning = true;
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _cts.Cancel();
                _isRunning = false;
            }
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var dbContext = new OrationiDatabaseContext())
                    {
                        var callbackMessages = dbContext
                            .CallbackMessages
                            .Where(m => m.CanBeSend)
                            .Where(m => !m.WasSend)
                            .Where(m => m.StatusCode == (int) MessageStatusCodes.InProgress)
                            .ToList();

                        callbackMessages.ForEach(callbackMessage =>
                        {
                            try
                            {
                                ProcessCallbackMessage(callbackMessage, dbContext);
                            }
                            catch (Exception ex)
                            {
                                callbackMessage.StatusCode = (int)MessageStatusCodes.Error;
                                callbackMessage.ErrorMessage = ex.Message;
                                callbackMessage.CanBeSend = false;
                                dbContext.SaveChanges();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"При обработке пакета callback-сообщений возникло исключение {ex.GetType().Name}: {ex.Message}");
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_checkFrequency));
                }
            }
            Log.Info("Завершение работы обработчика callback-сообщений");
        }

        private void ProcessCallbackMessage(CallbackMessage callbackMessage, OrationiDatabaseContext dbContext)
        {
            var callbackSettings = callbackMessage.Message.ExternalSystem.CallbackSettings;
            if (callbackSettings is null)
            {
                throw new InvalidOperationException("Невозможно выполнить отправку сообщения по причине отсутствия"
                + $" Callback-настроек для внешней системы {callbackMessage.Message.ExternalSystem.SystemName}");
            }

            switch (callbackSettings.MethodType.ToLower())
            {
                case "rest":
                    RestSending(callbackMessage.Message);
                    break;
                case "soap":
                    SoapSending(callbackMessage.Message);
                    break;
                default:
                    throw new InvalidOperationException($"Для типа отправки {callbackSettings.MethodType} отсутствует обработчик");
            }

            callbackMessage.StatusCode = (int)MessageStatusCodes.Processed;
            callbackMessage.CanBeSend = false;
            dbContext.SaveChanges();
        }

        private void RestSending(Message message)
        {
            var callbackSettings = message.ExternalSystem.CallbackSettings;
            var client = new RestClient(callbackSettings.RequestTargetUrl);
            var request = new RestRequest(Method.POST);

            if (callbackSettings.NeedAuthentification)
            {
                request.Credentials = new NetworkCredential(callbackSettings.UrlAccessUserName, callbackSettings.UrlAccessUserPassword);
            }

            var callbackRequest = new CallbackRequest
            {
                MessageId = message.Id,
                Code = message.RequestCodeId,
                Body = message.ResponseBody,
                IsError = message.StatusCode == (int)MessageStatusCodes.Error,
                ErrorMessage = message.ErrorMessage
            };

            request.AddJsonBody(callbackRequest.ToJson());
            var response = client.Execute<CallbackResponse>(request);

            if (response.Data is null)
            {
                throw new InvalidOperationException("На Callback-запрос не получен ответ, или ответ был пустым");
            }

            if (response.Data.IsError)
            {
                throw new InvalidOperationException($"На Callback-запрос получен ответ с ошибкой: {response.Data.ErorrMessage}");
            }
        }

        private CallbackResponse SoapSending(Message message)
        {
            throw new NotImplementedException("SoapSending не реализован");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

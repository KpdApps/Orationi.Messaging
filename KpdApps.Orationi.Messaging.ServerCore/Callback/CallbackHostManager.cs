using System;
using System.Linq;
using System.Net;
using System.Threading;
using KpdApps.Orationi.Messaging.Common;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using RestSharp;

namespace KpdApps.Orationi.Messaging.ServerCore.Callback
{
    public class CallbackHostManager
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly double _checkFrequency;

        public CallbackHostManager(double checkFrequency)
        {
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
            Run(_cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
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
                    // при попытке установления соединения, произошла ошибка такая-то такая-то, залогировать это
                    // и уйти в таймаут
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_checkFrequency));
                }
            }
        }

        private void ProcessCallbackMessage(CallbackMessage callbackMessage, OrationiDatabaseContext dbContext)
        {
            var callbackSettings = callbackMessage.Message.ExternalSystem.CallbackSettings;
            if (callbackSettings is null)
            {
                throw new InvalidOperationException("Невозможно выполнить отправку сообщения по причине отсутствия"
                + $" Callback-настроек для внешней системы {callbackMessage.Message.ExternalSystem.SystemName}");
            }

            CallbackResponse callbackResponse = null;

            switch (callbackSettings.MethodType.ToLower())
            {
                case "rest":
                    callbackResponse = RestSending(callbackMessage.Message);
                    break;
                case "soap":
                    callbackResponse = SoapSending(callbackMessage.Message);
                    break;
                default:
                    throw new InvalidOperationException($"Для типа отправки {callbackSettings.MethodType} отсутствует обработчик");
            }

            callbackMessage.StatusCode = (int)MessageStatusCodes.Processed;
            callbackMessage.CanBeSend = false;
            dbContext.SaveChanges();
        }

        private CallbackResponse RestSending(Message message)
        {
            var callbackSettings = message.ExternalSystem.CallbackSettings;
            var client = new RestClient(callbackSettings.RequestTargetUrl);
            var request = new RestRequest(Method.POST)
            {
                Credentials = new NetworkCredential(
                    callbackSettings.UrlAccessUserName, 
                    callbackSettings.UrlAccessUserPassword)
            };
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
                throw new InvalidOperationException($"На Callback-запрос не получен ответ, или ответ был пустым");
            }

            if (response.Data.IsError)
            {
                throw new InvalidOperationException($"На Callback-запрос получен ответ с ошибкой: {response.Data.ErorrMessage}");
            }

            return response.Data;
        }

        private CallbackResponse SoapSending(Message message)
        {
            throw new NotImplementedException("SoapSending не реализован");
        }
    }
}

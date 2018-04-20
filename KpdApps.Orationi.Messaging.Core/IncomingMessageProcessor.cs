using System;
using System.Linq;
using System.Reflection;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using KpdApps.Orationi.Messaging.Sdk.Attributes;

namespace KpdApps.Orationi.Messaging.Core
{
    public class IncomingMessageProcessor
    {
        private readonly OrationiDatabaseContext _dbContext;
        private readonly ExternalSystem _externalSystem;

        public IncomingMessageProcessor(OrationiDatabaseContext dbContext, ExternalSystem externalSystem)
        {
            _dbContext = dbContext;
            _externalSystem = externalSystem;
        }

        public Response Execute(Request request)
        {
            try
            {
                Response response = new Response();

                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = _externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = true
                };
                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient();
                client.Execute(message.RequestCodeId, message.Id);

                _dbContext.Entry(message).Reload();

                response = new Response
                {
                    Id = message.Id,
                    Body = message.ResponseBody
                };

                if (!string.IsNullOrEmpty(message.ErrorMessage))
                {
                    response.IsError = true;
                    response.Error = message.ErrorMessage;
                }

                return response;
            }
            catch (Exception ex)
            {
                return new Response() { IsError = true, Error = $"{ex.Message} {(ex.InnerException is null ? "" : ex.InnerException.Message)}" };
            }
        }

        public Response GetResponse(Guid requestId)
        {
            Response response = new Response();
            Message message = _dbContext.Messages.FirstOrDefault(m => m.Id == requestId);

            if (message is null)
            {
                response.Id = requestId;
                response.IsError = true;
                response.Error = $"Request {requestId} not found";
                return response;
            }

            //TODO: Обработка статуса сообщения, если еще не обработано возвращаем статус / ошибку
            return new Response() {
				Id = requestId,
				IsError = false,
				Error = null,
				Body = message.ResponseBody };
        }

        public ResponseId ExecuteAsync(Request request)
        {
            try
            {
                SetRequestCode(request);
                Message message = new Message
                {
                    RequestBody = request.Body,
                    RequestCodeId = request.Code,
                    ExternalSystemId = _externalSystem.Id,
                    RequestUser = request.UserName,
                    IsSyncRequest = false
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();

                RabbitClient client = new RabbitClient();
                client.PullMessage(message.RequestCodeId, message.Id);

                ResponseId response = new ResponseId();
                response.Id = message.Id;

                return response;
            }
            catch (Exception ex)
            {
                return new Response() {
					IsError = true,
					Error = $"{ex.Message} {(ex.InnerException is null ? "No inner exception" : ex.InnerException.Message)}"
				};
            }
        }

        public void SetRequestCode(Request request)
        {
            //Если указали алиас типа запроса, но не указали код - получаем код запроса
            if (!string.IsNullOrEmpty(request.Type) && request.Code == 0)
            {
                RequestCodeAlias requestCodeAlias = _dbContext.RequestCodeAliases.FirstOrDefault(rca => rca.Alias == request.Type);
                if (requestCodeAlias == null)
                {
                    throw new InvalidOperationException($"Invalid request type: {request.Type}.");
                }
                request.Code = requestCodeAlias.RequestCode;
            }
        }

        public ResponseXsd GetXsd(int requestCode)
        {
            var response = new ResponseXsd();

            var request = _dbContext
                .RequestCodes
                .FirstOrDefault(rc => rc.Id == requestCode);

            if (request is null)
            {
                response.IsError = true;
                response.Error = $"Для requestCode = {requestCode}, отсутствует запись в БД";
                return response;
            }

            var registeredPlugin = request
                .Workflows
                .FirstOrDefault()
                ?.WorkflowActions
                .OrderBy(wfa => wfa.Order)
                .FirstOrDefault()
                ?.PluginActionSet
                .PluginActionSetItems
                .OrderBy(pasi => pasi.Order)
                .FirstOrDefault()
                ?.RegisteredPlugin;

            if (registeredPlugin is null)
            {
                response.IsError = true;
                response.Error = $"Для requestCode = {requestCode}, не найден подходящий зарегистрированный плагин.{Environment.NewLine}Порядок поиска Workflows (1) — WorkflowActions (сортировка, 1) — PluginActionSet — PluginActionSetItems (сортировка, 1) — RegisteredPlugin";
                return response;
            }

            var assembly = Assembly.Load(registeredPlugin.PluginAssembly.Assembly);

            var pluginType = assembly.GetType(registeredPlugin.Class);

            response.RequestContract = ((ContractAttribute)pluginType.GetCustomAttribute(typeof(RequestContractAttribute)))
                ?.GetXsd(assembly);

			response.ResponseContract = ((ContractAttribute)pluginType.GetCustomAttribute(typeof(ResponseContractAttribute)))
                ?.GetXsd(assembly);

            return response;
        }

		public Response FileUpload(Guid messageId, string fileName, string fileType, byte[] fileAsArray)
		{
			var response = new Response();

			// Проверяем, что существует исходное сообщение, по которому создали связанную с файлом сущность.
			var createServiceMessage = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
			if (createServiceMessage == null)
			{
				response.IsError = true;
				response.Error = $"Не нашли сообщения с идентификатором {messageId}";
				return response;
			}

			// Сохраняем файл.
			FileStore fileStore = new FileStore
			{
				MessageId = messageId,
				FileName = fileName,
				CreatedOn = DateTime.Now
			};
			fileAsArray.CopyTo(fileStore.Data, 0);

			_dbContext.FileStores.Add(fileStore);
			_dbContext.SaveChanges();


			// Формируем сообщение для плагина перекладывания файла из БД в Шарик
			// TODO: плагину для работы надо ИД Сервиса и ИД файла. 
			// ИД сообщения, с которым связан файл, не очень подходит, тк pipeLineExecutionContext.FirstOrdefault тянет файл по ключу сообщения, 
			// а если файлов несколько и два плагина одновременно начнут тянуть файлы - могут оба потянуть один и тот же файл.
			// В итоге приходим к тому, что тут надо формировать свое тело запроса к плагину. Т.е. тащить в этот проект XSD? Или json нас устроит?
			var uploadMessage = new Message
			{
				RequestBody = createServiceMessage.ResponseBody, // В ответе создания услуги есть и messageId - нужен для получения файла, и serviceId - нужен для привязывания к нему файла
				RequestCodeId = 70001, // TODO: заглушка. На самом деле надо придумать нормальный код. И Я подозреваю, его нужно получать от клиента в теле json-объекта.
				ExternalSystemId = _externalSystem.Id,
				RequestUser = "Orationi.Messaging.Core",
				IsSyncRequest = false
			};

			_dbContext.Messages.Add(uploadMessage);
			_dbContext.SaveChanges();

			RabbitClient client = new RabbitClient();
			client.PullMessage(uploadMessage.RequestCodeId, uploadMessage.Id);

			response.Id = uploadMessage.Id;
			response.IsError = false;

			return response;
		}
    }
}
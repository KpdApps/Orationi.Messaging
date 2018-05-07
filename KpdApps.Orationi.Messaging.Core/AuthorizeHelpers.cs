using System;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;

namespace KpdApps.Orationi.Messaging.Core
{
    public static class AuthorizeHelpers
    {
        public static bool IsAuthorized<T>(
			OrationiDatabaseContext dbContext,
            string token,
            int requestCode,
            out T response,
            out ExternalSystem externalSystem)
        where T : ResponseBase, new()
        {
            response = new T();

            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(token))
            {
                errorList.Add($"Invalid token ({token}).");
            }

            externalSystem = dbContext
                .ExternalSystems
                .Where(extSys => extSys.Token == token)
                .FirstOrDefault(extSys => extSys.RequestCodes.Any(rc => rc.Id == requestCode));

            if (externalSystem is null)
            {
                errorList.Add($" External system is not exists for token ({token}).");
            }

            if (errorList.Count == 0) return true;

            response.IsError = true;
            response.Error = string.Join(" ", errorList);

            return false;
        }

        public static bool IsAuthorized<T>(
			OrationiDatabaseContext dbContext,
            string token,
            Guid messageId,
            out T response,
            out ExternalSystem externalSystem)
            where T : ResponseBase, new()
        {
            var message = dbContext.Messages.FirstOrDefault(m => m.Id == messageId);

            if (message is null)
            {
                response = new T
                {
                    IsError = true,
                    Error = $"Request {messageId} not found"
                };
                externalSystem = null;
                return false;
            }

            return IsAuthorized<T>(dbContext, token, message.RequestCodeId, out response, out externalSystem);
        }
    }
}
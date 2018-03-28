using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KpdApps.Orationi.Messaging.Core
{
    public static class AuthorizeHelpers
    {
        /// <summary>
        /// Проверика авторизации пользователя или системы при обращении к шине
        /// </summary>
        /// <param name="context">HttpContext <see cref="HttpContext"/></param>
        /// <param name="requestCode">Код запроса</param>
        /// <param name="token">Секретный ключ, по которому производится авторизация</param>
        /// <param name="response">Ответ</param>
        /// <param name="externalSystem">Внешняя система, которая выполнила запрос</param>
        /// <returns></returns>
        public static bool IsAuthorized(this HttpContext context,
            OrationiMessagingContext dbContext,
            int requestCode,
            ResponseBase response,
            out ExternalSystem externalSystem)
        {
            var errorList = new List<string>();

            var token = context.Request.Headers["Token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                errorList.Add($"Invalid token ({token}).");
            }

            externalSystem = (from extSys in dbContext.ExternalSystems
                join extSysReqCodes in dbContext.ExternalSystemRequestCodes on extSys.Id equals extSysReqCodes.ExternalSystemId
                join reqCode in dbContext.RequestCodes on extSysReqCodes.RequestCodeId equals reqCode.Id
                where reqCode.Id == requestCode && extSys.Token == token
                select new
                    {
                        ExternalSystem = extSys
                    })
                    .FirstOrDefault()
                    ?.ExternalSystem;

            if (externalSystem is null)
            {
                errorList.Add($" External system is not exists for token ({token}).");
            }

            if (errorList.Count == 0) return true;

            context.Response.StatusCode = 401;
            response.IsError = true;
            response.Error = string.Join(" ", errorList);

            return false;
        }
    }
}

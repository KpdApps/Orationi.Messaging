using System.Collections.Generic;
using System.Linq;
using System.Web;
using KpdApps.Orationi.Messaging.Common.Models;
using KpdApps.Orationi.Messaging.DataAccess.EF;
using KpdApps.Orationi.Messaging.DataAccess.EF.Models;

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
            OrationiDatabaseContext dbContext,
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

            externalSystem = dbContext
                .ExternalSystems
                .Where(extSys => extSys.Token == token)
                .FirstOrDefault(extSys => extSys.RequestCodes.Any(rc => rc.Id == requestCode));

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

using System.Collections.Generic;
using RRMonitoring.Identity.Application.Enums;

namespace RRMonitoring.Identity.Application.Constants;

public static class ExternalAuthErrors
{
	public static readonly Dictionary<ExternalAuthErrorCodes, string> ExternalErrorMessages = new()
	{
		{ ExternalAuthErrorCodes.Unauthorized, "Пользователь с данным логином/паролем не существует" },
		{ ExternalAuthErrorCodes.UserNotExist, "Пользователь с данным логином/паролем не найден" },
		{ ExternalAuthErrorCodes.Unknown, "Возникла непредвиденная ошибка" },
		{ ExternalAuthErrorCodes.UserBlocked, "Пользователь заблокирован. Свяжитесь с администратором" },
		{ ExternalAuthErrorCodes.UserNotApproved, "Пользователь с данным логином/паролем не существует" },
	};
}

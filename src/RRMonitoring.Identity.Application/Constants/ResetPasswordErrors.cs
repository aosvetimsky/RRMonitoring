using System.Collections.Generic;
using RRMonitoring.Identity.Application.Enums;

namespace RRMonitoring.Identity.Application.Constants;

public static class ResetPasswordErrors
{
	public static readonly Dictionary<ResetPasswordErrorCodes, string> ErrorMessages = new()
	{
		{ ResetPasswordErrorCodes.ExpiredPassword, "Срок действия пароль истёк. Задайте новый пароль" }
	};
}

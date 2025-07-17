using System;
using RRMonitoring.Identity.Application.Enums;

namespace RRMonitoring.Identity.Application.Services.Link;

public interface ILinkService
{
	string GenerateResetPasswordLink(Guid userId, string token, ResetPasswordErrorCodes? errorCode = null);

	string GenerateConfirmEmailLink(Guid userId, string token);
}

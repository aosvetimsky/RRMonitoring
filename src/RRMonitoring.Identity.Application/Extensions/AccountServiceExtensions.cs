using System;
using Nomium.Core.Security.Services.Account;

namespace RRMonitoring.Identity.Application.Extensions;

public static class AccountServiceExtensions
{
	public static Guid GetRequiredCurrentUserId(this IAccountService accountService)
	{
		var currentUserId = accountService.GetCurrentUserId();

		if (!currentUserId.HasValue)
		{
			throw new UnauthorizedAccessException("No userId was found in token");
		}

		return currentUserId.Value;
	}
}

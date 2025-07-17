using System;

namespace RRMonitoring.Identity.Application.Services.Agreement;

internal class VerifiedLogin
{
	public string UserId { get; set; }

	public DateTime ExpirationDate { get; set; }
}

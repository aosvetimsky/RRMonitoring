using System;

namespace RRMonitoring.Identity.Application.Features.Auth.TwoFactor;

public class ResendTwoFactorRequest
{
	public Guid UserId { get; set; }

	public string Token { get; set; }
}

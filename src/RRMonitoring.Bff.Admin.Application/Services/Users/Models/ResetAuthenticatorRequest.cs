using System;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class ResetAuthenticatorRequest
{
	public Guid UserId { get; set; }

	public string Code { get; set; }
}

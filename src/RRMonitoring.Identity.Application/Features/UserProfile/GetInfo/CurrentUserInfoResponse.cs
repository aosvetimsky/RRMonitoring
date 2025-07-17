using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Application.Features.UserProfile.GetInfo;

public class CurrentUserInfoResponse
{
	[Required]
	public Guid Id { get; set; }
	[Required]
	public string UserName { get; set; }
	[Required]
	public string Email { get; set; }
	[Required]
	public bool IsTwoFactorEnabled { get; set; }
	[Required]
	public bool IsAuthenticatorEnabled { get; set; }
	[Required]
	public string LastPasswordChangedDate { get; set; }
	public List<string> Permissions { get; set; }
}

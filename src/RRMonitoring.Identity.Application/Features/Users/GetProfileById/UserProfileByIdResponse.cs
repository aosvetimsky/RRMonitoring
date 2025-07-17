using System;
using System.Diagnostics.CodeAnalysis;

namespace RRMonitoring.Identity.Application.Features.Users.GetProfileById;

public sealed record UserProfileByIdResponse
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	[SuppressMessage("Performance", "CA1819:Properties should not return arrays")] // TODO: Fix and break backward compatibility
	public string[] Permissions { get; set; }
}

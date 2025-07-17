using System;

namespace RRMonitoring.Identity.Application.Features.Users.GetByUserNames;

public class UserByUserNameResponse
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string ExternalId { get; set; }
	public bool IsBlocked { get; set; }
	public byte? StatusId { get; set; }
	public byte? TypeId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class SearchUserStatusResponse
{
	[Required]
	public byte Id { get; set; }

	[Required]
	public string Name { get; set; }
}
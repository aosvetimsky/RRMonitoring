using System;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;

public class SearchManufacturersResponse
{
	[Required]
	public Guid Id { get; init; }

	[Required]
	public string Name { get; init; }
}

using System;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;

public class UpdateManufacturerRequest
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}

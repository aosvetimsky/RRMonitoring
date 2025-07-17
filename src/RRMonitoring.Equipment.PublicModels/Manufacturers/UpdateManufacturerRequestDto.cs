using System;

namespace RRMonitoring.Equipment.PublicModels.Manufacturers;

public class UpdateManufacturerRequestDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}

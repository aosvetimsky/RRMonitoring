using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

public class SearchFacilitiesResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public int PowerCapacity { get; set; }

	public int PowerReserve { get; set; }

	public int EquipmentQuantity { get; set; }

	public int EquipmentUnderRepairQuantity { get; set; }

	public int FreePlacesQuantity { get; set; }

	public SearchFacilitiesUserResponse Manager { get; set; }

	public SearchFacilitiesUserResponse DeputyManager { get; set; }

	public IReadOnlyList<SearchFacilitiesUserResponse> Technicians { get; set; }
}

public class SearchFacilitiesUserResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public bool IsAuthorized { get; set; }
}

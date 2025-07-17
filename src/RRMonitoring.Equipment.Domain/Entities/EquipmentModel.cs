using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class EquipmentModel : AuditableEntity
{
	public string Name { get; set; }

	public Guid ManufacturerId { get; set; }
	public Manufacturer Manufacturer { get; set; }

	public byte HashrateUnitId { get; set; }
	public HashrateUnit HashrateUnit { get; set; }
	
	public decimal NominalHashrate { get; set; }

	public int NominalPower { get; set; }

	public int MaxMotherBoardTemperature { get; set; }

	public int MaxProcessorTemperature { get; set; }

	public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

	public ICollection<FirmwareEquipmentModel> FirmwareEquipmentModels { get; set; } = new List<FirmwareEquipmentModel>();

	public ICollection<EquipmentModelCoin> EquipmentModelCoins { get; set; } = new List<EquipmentModelCoin>();
}

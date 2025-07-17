using System;

namespace RRMonitoring.Equipment.Domain.Entities;

public class EquipmentModelCoin
{
	public Guid EquipmentModelId { get; set; }
	public EquipmentModel EquipmentModel { get; set; }

	public byte CoinId { get; set; }
}

using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class EquipmentStatus(byte id, string name) : DictionaryEntity<byte>(id, name)
{
}

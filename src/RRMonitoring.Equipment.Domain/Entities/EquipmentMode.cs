using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class EquipmentMode(byte id, string name) : DictionaryEntity<byte>(id, name)
{
}

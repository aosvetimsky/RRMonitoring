using Nomium.Core.Data.Entities;

namespace RRMonitoring.Mining.Domain.Entities;

public class WorkerStatus(byte id, string name) : DictionaryEntity<byte>(id, name)
{
}

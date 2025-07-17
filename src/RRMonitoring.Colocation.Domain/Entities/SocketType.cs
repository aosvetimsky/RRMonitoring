using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class SocketType(byte id, string name, string code) : DictionaryEntity<byte>(id, name)
{
	public string Code { get; set; } = code;
}

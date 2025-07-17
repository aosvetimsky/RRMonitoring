using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class UserStatus : DictionaryEntity<byte>
{
	public UserStatus(byte id, string name)
		: base(id, name)
	{
	}
}

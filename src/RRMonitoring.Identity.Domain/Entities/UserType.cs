using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class UserType : DictionaryEntity<byte>
{
	public string Code { get; set; }

	public UserType(byte id, string name) : base(id, name)
	{
	}
}

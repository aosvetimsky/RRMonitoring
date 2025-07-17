using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class ExternalPermission : DictionaryEntity<byte>
{
	public ExternalPermission(byte id, string name, string code)
		: base(id, name)
	{
		Code = code;
	}

	public string Code { get; set; }
}

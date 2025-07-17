using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class ExternalSource : DictionaryEntity<byte>
{
	public string Code { get; set; }

	public ExternalSource(byte id, string name, string code)
		: base(id, name)
	{
		Code = code;
	}
}

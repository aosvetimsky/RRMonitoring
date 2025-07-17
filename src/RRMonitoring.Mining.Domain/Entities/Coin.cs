using Nomium.Core.Data.Entities;

namespace RRMonitoring.Mining.Domain.Entities;

public class Coin(byte id, string name, string ticker, string externalId) : DictionaryEntity<byte>(id, name)
{
	public string Ticker { get; set; } = ticker;

	public string ExternalId { get; set; } = externalId;
}

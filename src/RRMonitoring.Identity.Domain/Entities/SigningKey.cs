using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class SigningKey : EntityBase
{
	public DateTime CreationDate { get; private set; }

	public string Value { get; private set; }

	public SigningKey(Guid id, DateTime creationDate, string value)
	{
		Id = id;
		CreationDate = creationDate;
		Value = value;
	}

	public SigningKey(RsaSecurityKey rsaSecurityKey)
	{
		Id = Guid.Parse(rsaSecurityKey.KeyId);
		Value = JsonConvert.SerializeObject(rsaSecurityKey.Parameters);
	}

	public RsaSecurityKey ToRsa()
	{
		try
		{
			var parameters = JsonConvert.DeserializeObject<RSAParameters>(Value);

			var key = new RsaSecurityKey(parameters) { KeyId = Id.ToString() };

			return key;
		}
		catch (JsonReaderException e)
		{
			throw new Exception($"Deserialization of signing key {Value} failed", e);
		}
	}
}

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RRMonitoring.Identity.Application.Helpers;

public static class ReferralCodeGenerator
{
	private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

	public static List<string> GenerateReferralCodes(int amountOfCodes, int lengthOfCode)
	{
		var codes = new List<string>();

		using var crypto = RandomNumberGenerator.Create();
		for (var i = 0; i < amountOfCodes; i++)
		{
			var data = new byte[lengthOfCode];
			crypto.GetBytes(data);

			var result = new StringBuilder(lengthOfCode);

			foreach (var b in data)
			{
				result.Append(_chars[b % _chars.Length]);
			}

			codes.Add(result.ToString());
		}

		return codes;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

internal static class IpRangeEnumerator
{
	public static IEnumerable<string> Expand(string mask, int hardLimit)
	{
		var octets = mask.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (octets.Length != 4)
		{
			yield break;
		}

		var parts = octets.Select(ParsePart).ToArray();
		var produced = 0;

		foreach (var o1 in parts[0])
		foreach (var o2 in parts[1])
		foreach (var o3 in parts[2])
		foreach (var o4 in parts[3])
		{
			if (produced++ >= hardLimit)
			{
				yield break;
			}

			yield return $"{o1}.{o2}.{o3}.{o4}";
		}
	}

	private static IEnumerable<int> ParsePart(string part)
	{
		if (part == "*")
		{
			return Enumerable.Range(0, 256);
		}

		if (part.Contains('-'))
		{
			var p = part.Split('-', 2);
			int l = int.Parse(p[0]), r = int.Parse(p[1]);
			return Enumerable.Range(l, r - l + 1);
		}

		return part.Contains(',')
			? part.Split(',').Select(int.Parse)
			: [int.Parse(part)];
	}
}

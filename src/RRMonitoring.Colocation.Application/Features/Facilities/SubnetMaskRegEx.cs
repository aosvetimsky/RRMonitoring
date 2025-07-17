using System.Text.RegularExpressions;

namespace RRMonitoring.Colocation.Application.Features.Facilities;

internal static class SubnetMaskRegEx
{
	private static readonly string _num = @"(255|254|252|248|240|224|192|128|0+)";

	public static readonly Regex RegEx = new Regex("^" + _num + @"\." + _num + @"\." + _num + @"\." + _num + "$");
}

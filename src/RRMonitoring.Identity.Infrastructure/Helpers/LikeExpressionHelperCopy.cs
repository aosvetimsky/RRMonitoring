using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace RRMonitoring.Identity.Infrastructure.Helpers;

// TODO: Update Nomium.Core.Identity to .NET 7 and use LikeExpressionHelper instead
internal static class LikeExpressionHelperCopy
{
	private static readonly char[] _charactersForEscaping = { '%', '_', '\\' };

	public static readonly string EscapeCharacter = "\\";

	public static MethodCallExpression BuildSafeILikeExpression(string keyword, MemberExpression property,
		CancellationToken cancellationToken = default)
	{
		var likeMethod = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike",
			new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(string) })!;

		return Expression.Call(instance: null, method: likeMethod, Expression.Constant(EF.Functions),
			property, Expression.Constant(ToSubstringPattern(keyword)), Expression.Constant(EscapeCharacter));
	}

	public static string ToStringPattern(string originalString)
	{
		if (string.IsNullOrEmpty(originalString))
		{
			return "";
		}

		return EscapeSpecialCharacters(originalString);
	}

	public static string ToSubstringPattern(string originalString)
	{
		if (string.IsNullOrEmpty(originalString))
		{
			return "%";
		}

		return $"%{EscapeSpecialCharacters(originalString)}%";
	}

	private static string EscapeSpecialCharacters(string originalString)
	{
		var firstEscapeSymbol = originalString.IndexOfAny(_charactersForEscaping);
		if (firstEscapeSymbol == -1)
		{
			return originalString;
		}

		var stringBuilder = new StringBuilder(originalString.Substring(0, firstEscapeSymbol));
		for (var i = firstEscapeSymbol; i < originalString.Length; i++)
		{
			if (_charactersForEscaping.Contains(originalString[i]))
			{
				stringBuilder.Append(EscapeCharacter);
			}

			stringBuilder.Append(originalString[i]);
		}

		return stringBuilder.ToString();
	}
}

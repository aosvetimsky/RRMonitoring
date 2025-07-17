using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts.PostgreSql;

internal static class PostgreSqlLexer
{
	public static IEnumerable<Token> GetTokens(string input)
	{
		var lastPos = 0;
		var match = _combinedRegex.Match(input);
		while (match.Success)
		{
			if (lastPos < match.Index)
			{
				yield return new Token(TokenType.Error, $"Skipped invalid lexeme '{input[lastPos..match.Index]}'");
				yield break;
			}

			Group foundGroup = null;

			foreach (var group in _groups)
			{
				if (match.Groups[group.Name].Success)
				{
					if (foundGroup is not null)
					{
						throw new InvalidOperationException("Bad regex found multiple matches");
					}

					foundGroup = group;
				}
			}

			if (foundGroup is null)
			{
				throw new InvalidOperationException("Bad regex found no matches");
			}

			if (foundGroup.TokenType == TokenType.Unsupported)
			{
				yield return new Token(TokenType.Error, $"Unsupported lexeme '{match.Value}'");
				yield break;
			}

			if (foundGroup.TokenType == TokenType.InternalDollarQuotedStringStart)
			{
				var endIndex = input.IndexOf(match.Value, match.Index + match.Length);
				if (endIndex == -1)
				{
					yield return new Token(TokenType.Error, $"Unterminated dollar-quoted string");
					yield break;
				}
				else
				{
					var lexemeEndIndex = endIndex + match.Length;
					var lexeme = input.Substring(match.Index, lexemeEndIndex - match.Index);
					yield return new Token(TokenType.DollarQuotedString, lexeme);
					lastPos = lexemeEndIndex;
					match = _combinedRegex.Match(input, lexemeEndIndex);
				}
			}
			else
			{
				yield return new Token(foundGroup.TokenType, match.Value);
				lastPos = match.Index + match.Length;
				match = match.NextMatch();
			}
		}

		if (lastPos < input.Length)
		{
			yield return new Token(TokenType.Error, $"Skipped invalid lexeme '{input[lastPos..]}'");
			yield break;
		}
	}

	private sealed record TokenPattern(TokenType Type, string Pattern);

	/// <summary>
	/// See <a href="https://www.postgresql.org/docs/16/sql-syntax-lexical.html"/>
	/// and <a href="https://github.com/postgres/postgres/blob/REL_16_2/src/backend/parser/scan.l"/>
	/// </summary>
	private static readonly IReadOnlyList<TokenPattern> _tokenPatterns = new TokenPattern[] {
		new (TokenType.Unsupported, /* language=regex */ @"
			/\* |
			0x | 0o | 0b |
			[eE]' |
			[bB]' |
			[xX]'
		"),
		new (TokenType.Spaces, /* language=regex */ @"[ \t\f]+"),
		new (TokenType.Comment, /* language=regex */ @"--[^\r\n]*"),
		new (TokenType.LineBreak, /* language=regex */ @"\r?\n|\r"),
		new (TokenType.Identifier, /* language=regex */ @"[a-zA-Z_][a-zA-Z_0-9$]*"),
		new (TokenType.QuotedIdentifier, /* language=regex */ @"""(""""|[^""\u0000])*"""),
		new (TokenType.PositionalParameter, /* language=regex */ @"\$\d+"),
		new (TokenType.String, /* language=regex */ @"'(''|[^'])*'"),
		new (TokenType.InternalDollarQuotedStringStart, /* language=regex */ @"\$([a-zA-Z_][a-zA-Z_0-9]*)?\$"),
		new (TokenType.Numeric, /* language=regex */ @"
			\d+ |
			\d+\.(\d+)?([eE][+-]?\d+)? |
			(\d+)?\.\d+([eE][+-]?\d+)? |
			\d+e[+-]?\d+
		"),
		new (TokenType.Operator, /* language=regex */ @"
			:= | :: | [()[\],;:*.] |
			[+\-*/<>=~!@\#%^&|`?]+
		"),
	};

	private static readonly Regex _combinedRegex = new(
		string.Join("\n|\n", _tokenPatterns.Where(x => x.Pattern is not null).Select(x => $"(?<{x.Type}>{x.Pattern})")),
		LexemeRegexOptions
	);

	private const RegexOptions LexemeRegexOptions =
		RegexOptions.ExplicitCapture |
		RegexOptions.Multiline |
		RegexOptions.Compiled |
		RegexOptions.Singleline |
		RegexOptions.IgnorePatternWhitespace |
		RegexOptions.CultureInvariant |
		RegexOptions.None;

	private sealed record Group(TokenType TokenType, string Name);

	private static readonly IReadOnlyList<Group> _groups = _tokenPatterns
		.Select(x => new Group(x.Type, x.Type.ToString()))
		.ToList();
}

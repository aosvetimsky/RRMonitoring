using System;
using System.Collections.Generic;
using System.Linq;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts.PostgreSql;

internal static class PostgreSqlQuerySplitter
{
	public delegate Exception MakeErrorDelegate(int lineNumber, int column, string message);

	public static IEnumerable<StartupScriptSqlQuery> Split(string text, MakeErrorDelegate makeError)
	{
		using var tokenEnumerator = PostgreSqlLexer.GetTokens(text).GetEnumerator();

		var eof = new Token(TokenType.Eof, "");
		var currentToken = eof;
		var currentTokenLocation = new Location(0, 0);

		void ReadNextToken()
		{
			if (currentToken.Type != TokenType.Eof)
			{
				var lexeme = currentToken.Lexeme;

				if (lexeme.Contains('\r'))
				{
					throw new NotSupportedException("Only Unix line endings are supported");
				}

				var startIndex = 0;
				var lastLineBreakIndex = -1;
				while (true)
				{
					var lineBreakIndex = lexeme.IndexOf('\n', startIndex);
					if (lineBreakIndex == -1)
					{
						break;
					}
					else
					{
						lastLineBreakIndex = lineBreakIndex;
						startIndex = lineBreakIndex + 1;
						currentTokenLocation = new Location(currentTokenLocation.LineIndex + 1, 0);
					}
				}

				var newColumnIndex = lastLineBreakIndex != -1
					? lexeme.Length - lastLineBreakIndex - 1
					: currentTokenLocation.ColumnIndex + lexeme.Length;
				currentTokenLocation = currentTokenLocation with { ColumnIndex = newColumnIndex };
			}

			currentToken = tokenEnumerator.MoveNext() ? tokenEnumerator.Current : eof;
			if (currentToken.Type == TokenType.Error)
			{
				throw makeError(
					lineNumber: currentTokenLocation.LineIndex + 1,
					column: currentTokenLocation.ColumnIndex + 1,
					message: currentToken.Lexeme
				);
			}
		}

		ReadNextToken();

		var currentSqlStart = new Location();
		var currentSqlTokens = new List<Token>();

		while (currentToken.Type != TokenType.Eof)
		{
			while (IsSkippable(currentToken))
			{
				ReadNextToken();
			}

			currentSqlStart = currentTokenLocation;
			currentSqlTokens.Clear();

			while (!(currentToken.Lexeme == ";" || currentToken.Type == TokenType.Eof))
			{
				currentSqlTokens.Add(currentToken);
				ReadNextToken();
			}

			ReadNextToken();

			while (currentSqlTokens.Count != 0 && IsSkippable(currentSqlTokens[currentSqlTokens.Count - 1]))
			{
				currentSqlTokens.RemoveAt(currentSqlTokens.Count - 1);
			}

			if (currentSqlTokens.Count != 0)
			{
				var currentSql = string.Concat(currentSqlTokens.Select(x => x.Lexeme));

				yield return new StartupScriptSqlQuery
				{
					Text = currentSql,
					StartLineNumber = currentSqlStart.LineIndex + 1,
					StartColumn = currentSqlStart.ColumnIndex + 1,
				};
			}
		}

		static bool IsSkippable(Token token)
		{
			return token.Type is TokenType.Spaces or TokenType.LineBreak or TokenType.Comment;
		}
	}

	private readonly record struct Location(int LineIndex, int ColumnIndex);
}

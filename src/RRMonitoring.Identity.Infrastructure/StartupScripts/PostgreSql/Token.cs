namespace RRMonitoring.Identity.Infrastructure.StartupScripts.PostgreSql;

internal sealed record Token(
	TokenType Type,
	string Lexeme
);

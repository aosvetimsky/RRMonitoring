namespace RRMonitoring.Identity.Infrastructure.StartupScripts.PostgreSql;

internal enum TokenType
{
	Spaces,
	LineBreak,
	Comment,
	Identifier,
	QuotedIdentifier,
	PositionalParameter,
	String,
	DollarQuotedString,
	InternalDollarQuotedStringStart,
	Numeric,
	Operator,
	Unsupported,
	Error,
	Eof,
}

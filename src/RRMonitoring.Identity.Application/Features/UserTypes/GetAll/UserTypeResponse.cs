namespace RRMonitoring.Identity.Application.Features.UserTypes.GetAll;

public sealed record UserTypeResponse
{
	public byte Id { get; init; }
	public string Name { get; init; }
	public string Code { get; init; }
}

using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Roles.Create;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
	public CreateRoleRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
		RuleFor(x => x.PermissionIds).NotEmpty();
	}
}

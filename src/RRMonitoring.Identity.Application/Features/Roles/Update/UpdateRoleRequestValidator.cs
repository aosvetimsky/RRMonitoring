using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Roles.Update;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
	public UpdateRoleRequestValidator()
	{
		RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
		RuleFor(x => x.PermissionIds).NotEmpty();
	}
}

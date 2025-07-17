using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Update;

public class UpdateManufacturerRequestValidator : AbstractRequestValidator<UpdateManufacturerRequest, UpdateManufacturerRequestDto>
{
	public UpdateManufacturerRequestValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
	}
}

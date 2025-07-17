using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Create;

public class CreateManufacturerRequestValidator : AbstractRequestValidator<CreateManufacturerRequest, CreateManufacturerRequestDto>
{
	public CreateManufacturerRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
	}
}

using System.Linq;
using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.Update;

public class UpdateFirmwareRequestValidator : AbstractRequestValidator<UpdateFirmwareRequest, UpdateFirmwareRequestDto>
{
	public UpdateFirmwareRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Version)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Comment)
			.MaximumLength(256);
		RuleFor(x => x.EquipmentModelIds)
			.Must(list => list.Any())
			.WithMessage("At least one equipment model is required.");

		RuleFor(x => x.EquipmentModelIds)
			.Custom((list, context) =>
			{
				if (list.GroupBy(x => x).Where(g => g.Count() > 1).Any())
				{
					context.AddFailure("Duplicate equipment models are not allowed.");
				}
			});
	}
}

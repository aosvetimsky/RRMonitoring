using System.Linq;
using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Create;

public class CreateEquipmentModelRequestValidator : AbstractRequestValidator<CreateEquipmentModelRequest, CreateEquipmentModelRequestDto>
{
	public CreateEquipmentModelRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
		RuleFor(x => x.CoinIds)
			.Must(list => list.Any())
			.WithMessage("At least one coin is required.");
		RuleFor(x => x.CoinIds)
			.Custom((list, context) =>
			{
				if (list.GroupBy(ca => ca).Where(g => g.Count() > 1).Any())
				{
					context.AddFailure("Duplicate coins are not allowed.");
				}
			});
	}
}

using System.Linq;
using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Update;

public class UpdateEquipmentModelRequestValidator : AbstractRequestValidator<UpdateEquipmentModelRequest, UpdateEquipmentModelRequestDto>
{
	public UpdateEquipmentModelRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
		RuleFor(x => x.CoinIds)
			.Must(list => list.Any())
			.WithMessage("At least one coin is required.");
		RuleFor(x => x.CoinIds)
			.Custom((list, context) =>
			{
				if (list.GroupBy(c => c).Any(g => g.Count() > 1))
				{
					context.AddFailure("Duplicate coins are not allowed.");
				}
			});
	}
}

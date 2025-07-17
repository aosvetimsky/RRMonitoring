using System.Linq;
using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Update;

public class UpdateFacilityRequestValidator : AbstractRequestValidator<UpdateFacilityRequest, UpdateFacilityRequestDto>
{
	public UpdateFacilityRequestValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.PowerCapacity)
			.NotEmpty();

		RuleFor(x => x.ManagerId)
			.NotEmpty();

		RuleFor(x => x.DeputyManagerId)
			.NotEmpty();

		RuleFor(x => x.Subnet)
			.MaximumLength(15)
			.Matches(SubnetMaskRegEx.RegEx)
			.WithMessage("Subnet is not valid");

		RuleFor(x => x.TechnicianIds)
			.Must(list => list.Any())
			.WithMessage("At least one technician is required.");

		RuleFor(x => x.TechnicianIds)
			.Custom((list, context) =>
			{
				if (list.GroupBy(x => x).Where(g => g.Count() > 1).Any())
				{
					context.AddFailure("Duplicate technicians are not allowed.");
				}
			});
	}
}

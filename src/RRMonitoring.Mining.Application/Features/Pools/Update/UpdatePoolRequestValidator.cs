using System.Linq;
using FluentValidation;
using Nomium.Core.MediatR;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Application.Features.Pools.Update;

public class UpdatePoolRequestValidator : AbstractRequestValidator<UpdatePoolRequest, UpdatePoolRequestDto>
{
	public UpdatePoolRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);
		RuleFor(x => x.CoinAddresses)
			.Must(list => list.Any())
			.WithMessage($"At least one {nameof(PoolCoinAddress)} is required.");

		RuleForEach(x => x.CoinAddresses)
			.ChildRules(coinAddress =>
			{
				coinAddress
					.RuleFor(ca => ca.FirstAddress)
					.NotEmpty();
			});

		RuleFor(x => x.CoinAddresses)
			.Custom((list, context) =>
			{
				if (list.GroupBy(ca => ca.CoinId).Any(g => g.Count() > 1))
				{
					context.AddFailure("Duplicate coins are not allowed.");
				}
			});
	}
}

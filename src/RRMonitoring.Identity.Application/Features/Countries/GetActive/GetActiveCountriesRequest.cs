using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PhoneNumbers;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Countries.GetActive;

public record GetActiveCountriesRequest : IRequest<List<CountryResponse>>;

public class GetActiveCountriesHandler : IRequestHandler<GetActiveCountriesRequest, List<CountryResponse>>
{
	private const string CountriesCacheKey = "countries";

	private readonly ICountryRepository _countryRepository;
	private readonly PhoneNumberUtil _phoneUtil;
	private readonly IMemoryCache _memoryCache;

	private readonly TimeSpan _longCacheDuration;

	public GetActiveCountriesHandler(
		ICountryRepository countryRepository,
		PhoneNumberUtil phoneUtil,
		IMemoryCache memoryCache,
		IOptions<CacheConfiguration> options)
	{
		_countryRepository = countryRepository;
		_phoneUtil = phoneUtil;
		_memoryCache = memoryCache;

		_longCacheDuration = options.Value.MemoryCacheLongLifetime;
	}

	public async Task<List<CountryResponse>> Handle(
		GetActiveCountriesRequest request,
		CancellationToken cancellationToken)
	{
		return await _memoryCache.GetOrCreateAsync(CountriesCacheKey, async entry =>
		{
			entry.AbsoluteExpirationRelativeToNow = _longCacheDuration;

			var countries = await _countryRepository.GetActive(cancellationToken);

			return countries.Select(country =>
				{
					var phoneCode = _phoneUtil.GetCountryCodeForRegion(country.Code);
					var exampleNumber = _phoneUtil.GetExampleNumberForType(country.Code, PhoneNumberType.MOBILE);

					if (phoneCode == 0 || exampleNumber is null)
					{
						return null;
					}

					var formattedNumber = _phoneUtil.Format(exampleNumber, PhoneNumberFormat.E164);

					return new CountryResponse
					{
						Id = country.Id,
						Name = country.Name,
						Code = country.Code,
						PhoneCode = phoneCode,
						PhoneExample = formattedNumber
					};
				})
				.Where(response => response != null)
				.ToList();
		});
	}
}

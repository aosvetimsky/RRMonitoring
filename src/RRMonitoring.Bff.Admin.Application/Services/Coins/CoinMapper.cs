using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.Coins.Models;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Bff.Admin.Application.Services.Coins;

public class CoinMapper : Profile
{
	public CoinMapper()
	{
		CreateMap<CoinResponseDto, CoinResponse>();
	}
}

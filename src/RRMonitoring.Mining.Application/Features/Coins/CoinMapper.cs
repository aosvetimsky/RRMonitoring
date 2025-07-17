using AutoMapper;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Mining.Application.Features.Coins;

public class CoinMapper : Profile
{
	public CoinMapper()
	{
		CreateMap<Coin, CoinResponseDto>();
		CreateMap<Coin, CoinByIdResponseDto>();
	}
}
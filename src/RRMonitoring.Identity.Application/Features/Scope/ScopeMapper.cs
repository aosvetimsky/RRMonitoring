using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using RRMonitoring.Identity.Application.Features.Scope.GetAvailable;

namespace RRMonitoring.Identity.Application.Features.Scope;

internal class ScopeMapper : Profile
{
	public ScopeMapper()
	{
		CreateMap<ApiScope, ScopeResponse>();
	}
}

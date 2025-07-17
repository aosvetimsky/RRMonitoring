using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Identity.Application.Features.Tenants.Create;
using RRMonitoring.Identity.Application.Features.Tenants.GetByCode;
using RRMonitoring.Identity.Application.Features.Tenants.GetById;
using RRMonitoring.Identity.Application.Features.Tenants.Search;
using RRMonitoring.Identity.Application.Features.Tenants.Update;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.Tenants;

internal class TenantMapper : Profile
{
	public TenantMapper()
	{
		CreateMap<CreateTenantRequest, Tenant>();

		CreateMap<UpdateTenantRequest, Tenant>();

		CreateMap<Tenant, TenantResponse>();
		CreateMap<Tenant, TenantByCodeResponse>();

		CreateMap<SearchTenantsRequest, SearchTenantsCriteria>();
		CreateMap<PagedList<Tenant>, PagedList<SearchTenantsResponseItem>>();
		CreateMap<Tenant, SearchTenantsResponseItem>();
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

namespace RRMonitoring.Identity.Infrastructure.Database;

public class IdentityContext
	: IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
			IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>,
		IConfigurationDbContext,
		IPersistedGrantDbContext
{
	public DbSet<Client> Clients { get; set; }
	public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
	public DbSet<IdentityResource> IdentityResources { get; set; }
	public DbSet<ApiResource> ApiResources { get; set; }
	public DbSet<ApiScope> ApiScopes { get; set; }
	public DbSet<PersistedGrant> PersistedGrants { get; set; }
	public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
	public DbSet<Country> Countries { get; set; }

	public IdentityContext(DbContextOptions<IdentityContext> options)
		: base(options)
	{
	}

	public Task<int> SaveChangesAsync()
	{
		return SaveChangesAsync(CancellationToken.None);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		this.SetAuditableEntitiesCreateUpdateDates();

		return base.SaveChangesAsync(cancellationToken);
	}

	public override int SaveChanges()
	{
		this.SetAuditableEntitiesCreateUpdateDates();

		return base.SaveChanges();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseSnakeCaseNamingConvention();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		ConfigureConfigurationEntities(builder);
		ConfigureOperationalEntities(builder);
		ConfigureIdentityEntities(builder);
	}

	private static void ConfigureIdentityEntities(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CountryConfiguration());
		modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
		modelBuilder.ApplyConfiguration(new UserEventConfiguration());
		modelBuilder.ApplyConfiguration(new UsedUserPasswordConfiguration());
		modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
		modelBuilder.ApplyConfiguration(new UserStatusConfiguration());
		modelBuilder.ApplyConfiguration(new SigningKeysConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claim");
		modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claim");
		modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_token");
		modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_login");

		modelBuilder.ApplyConfiguration(new PermissionConfiguration());
		modelBuilder.ApplyConfiguration(new PermissionGroupConfiguration());
		modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
		modelBuilder.ApplyConfiguration(new ScopePermissionConfiguration());
		modelBuilder.ApplyConfiguration(new PermissionGrantConfiguration());
		modelBuilder.ApplyConfiguration(new PermissionGrantPermissionConfiguration());

		modelBuilder.ApplyConfiguration(new TenantConfiguration());
		modelBuilder.ApplyConfiguration(new TenantUserConfiguration());
		modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
		modelBuilder.ApplyConfiguration(new ExternalSourceConfiguration());
		modelBuilder.ApplyConfiguration(new ExternalPermissionConfiguration());
	}

	private static void ConfigureConfigurationEntities(ModelBuilder modelBuilder)
	{
		var storeOptions = new ConfigurationStoreOptions
		{
			ApiResource = new TableConfiguration("api_resource"),
			ApiResourceClaim = new TableConfiguration("api_resource_claim"),
			ApiResourceProperty = new TableConfiguration("api_resource_property"),
			ApiResourceScope = new TableConfiguration("api_resource_scope"),
			ApiResourceSecret = new TableConfiguration("api_resource_secret"),
			ApiScope = new TableConfiguration("api_scope"),
			ApiScopeClaim = new TableConfiguration("api_scope_claim"),
			ApiScopeProperty = new TableConfiguration("api_scope_property"),
			Client = new TableConfiguration("client"),
			ClientClaim = new TableConfiguration("client_claim"),
			ClientCorsOrigin = new TableConfiguration("client_cors_origin"),
			ClientGrantType = new TableConfiguration("client_grant_type"),
			ClientIdPRestriction = new TableConfiguration("client_id_p_restriction"),
			ClientPostLogoutRedirectUri = new TableConfiguration("client_post_logout_redirect_uri"),
			ClientProperty = new TableConfiguration("client_property"),
			ClientRedirectUri = new TableConfiguration("client_redirect_uri"),
			ClientScopes = new TableConfiguration("client_scope"),
			ClientSecret = new TableConfiguration("client_secret"),
			IdentityResource = new TableConfiguration("identity_resource"),
			IdentityResourceClaim = new TableConfiguration("identity_resource_claim"),
			IdentityResourceProperty = new TableConfiguration("identity_resource_property")
		};

		modelBuilder.ConfigureClientContext(storeOptions);
		modelBuilder.ConfigureResourcesContext(storeOptions);
	}

	private static void ConfigureOperationalEntities(ModelBuilder modelBuilder)
	{
		var operationalStoreOptions = new OperationalStoreOptions
		{
			DeviceFlowCodes = new TableConfiguration("device_flow_code"),
			PersistedGrants = new TableConfiguration("persisted_grant")
		};

		modelBuilder.ConfigurePersistedGrantContext(operationalStoreOptions);
	}
}

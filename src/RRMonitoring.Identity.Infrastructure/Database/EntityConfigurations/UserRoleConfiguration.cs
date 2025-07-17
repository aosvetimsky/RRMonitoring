using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.ToTable("user_role");

		builder.HasOne(x => x.Role)
			.WithMany(x => x.UserRoles)
			.HasForeignKey(x => x.RoleId);

		builder.HasOne(x => x.User)
			.WithMany(x => x.UserRoles)
			.HasForeignKey(x => x.UserId);
	}
}

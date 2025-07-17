namespace RRMonitoring.Identity.Api.Security;

public static class Permissions
{
	// Администрирование
	public const string RoleRead = "role_read";
	public const string RoleManage = "role_manage";

	public const string UserRead = "user_read";
	public const string UserManage = "user_manage";
	public const string UserBlock = "user_block";

	public const string TenantRead = "tenant_read";
	public const string TenantManage = "tenant_manage";

	public const string LoginAsUser = "login_as_user";
}

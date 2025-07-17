using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddIdentitySwaggerClientAndPermissions : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                DO $$

                DECLARE
                    apiResourceId int;
                    clientId int;
                    scopeId int;
                    permissionGroupId int;
                    permissionId uuid;

                BEGIN

                    -- Add ApiScopes
                    INSERT INTO api_scope (
                            id,
                            enabled,
                            name,
                            display_name,
                            description,
                            required,
                            emphasize,
                            show_in_discovery_document
                        )
                    VALUES (
                            DEFAULT,
                            true,
                            'openid',
                            'Open ID Connect scope',
                            null,
                            true,
                            false,
                            true
                        );

                    INSERT INTO api_scope (
                            id,
                            enabled,
                            name,
                            display_name,
                            description,
                            required,
                            emphasize,
                            show_in_discovery_document
                        )
                    VALUES (
                            DEFAULT,
                            true,
                            'identity_api',
                            'Identity API scope',
                            null,
                            true,
                            false,
                            true
                        );

                    -- Add ApiResources
                    INSERT INTO api_resource (
                            id,
                            enabled,
                            name,
                            display_name,
                            description,
                            allowed_access_token_signing_algorithms,
                            show_in_discovery_document,
                            created,
                            updated,
                            last_accessed,
                            non_editable
                        )
                    VALUES (
                            DEFAULT,
                            true,
                            'identity_api',
                            'Identity API',
                            null,
                            null,
                            true,
                            now(),
                            null,
                            null,
                            false
                        )
                    RETURNING id INTO apiResourceId;

                    INSERT INTO api_resource_scope (id, scope, api_resource_id)
                    VALUES (DEFAULT, 'openid', apiResourceId);

                    INSERT INTO api_resource_scope (id, scope, api_resource_id)
                    VALUES (DEFAULT, 'identity_api', apiResourceId);

                    -- Add Clients
                    INSERT INTO client (
                            id,
                            enabled,
                            client_id,
                            protocol_type,
                            require_client_secret,
                            client_name,
                            description,
                            client_uri,
                            logo_uri,
                            require_consent,
                            allow_remember_consent,
                            always_include_user_claims_in_id_token,
                            require_pkce,
                            allow_plain_text_pkce,
                            require_request_object,
                            allow_access_tokens_via_browser,
                            front_channel_logout_uri,
                            front_channel_logout_session_required,
                            back_channel_logout_uri,
                            back_channel_logout_session_required,
                            allow_offline_access,
                            identity_token_lifetime,
                            allowed_identity_token_signing_algorithms,
                            access_token_lifetime,
                            authorization_code_lifetime,
                            consent_lifetime,
                            absolute_refresh_token_lifetime,
                            sliding_refresh_token_lifetime,
                            refresh_token_usage,
                            update_access_token_claims_on_refresh,
                            refresh_token_expiration,
                            access_token_type,
                            enable_local_login,
                            include_jwt_id,
                            always_send_client_claims,
                            client_claims_prefix,
                            pair_wise_subject_salt,
                            created,
                            updated,
                            last_accessed,
                            user_sso_lifetime,
                            user_code_type,
                            device_code_lifetime,
                            non_editable
                        )
                    VALUES (
                            DEFAULT,
                            true,
                            'swagger',
                            'oidc',
                            true,
                            null,
                            null,
                            null,
                            null,
                            false,
                            true,
                            false,
                            true,
                            false,
                            false,
                            false,
                            null,
                            true,
                            null,
                            true,
                            false,
                            300,
                            null,
                            3600,
                            300,
                            null,
                            2592000,
                            1296000,
                            1,
                            false,
                            1,
                            0,
                            true,
                            true,
                            false,
                            'client_',
                            null,
                            now(),
                            null,
                            null,
                            null,
                            null,
                            300,
                            true
                        )
                    RETURNING id INTO clientId;

                    INSERT INTO client_secret (
                            id,
                            client_id,
                            description,
                            value,
                            expiration,
                            type,
                            created
                        )
                    VALUES (
                            DEFAULT,
                            clientId,
                            null,
                            '0vBNbW+j67XG0NTjIbG0wObtcfrhddEF/0kgtt2l9kY=',
                            null,
                            'SharedSecret',
                            now()
                        );

                    INSERT INTO client_grant_type (id, grant_type, client_id)
                    VALUES (DEFAULT, 'authorization_code', clientId);

                    INSERT INTO client_scope (id, scope, client_id)
                    VALUES (DEFAULT, 'openid', clientId);

                    INSERT INTO client_scope (id, scope, client_id)
                    VALUES (DEFAULT, 'identity_api', clientId);

                    INSERT INTO client_redirect_uri (id, redirect_uri, client_id)
                    VALUES (
                            DEFAULT,
                            'http://localhost:5000/swagger/oauth2-redirect.html',
                            clientId
                        );

                    -- Группа администрирования
                    INSERT INTO permission_group (id, name)
                    VALUES (DEFAULT, 'Администрирование')
                    RETURNING id INTO permissionGroupId;

                    SELECT id INTO scopeId
                    FROM api_scope
                    WHERE name = 'identity_api'
                    ORDER BY name
                    LIMIT 1;

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'user_read',
                            'Просмотр справочника пользователей',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'user_manage',
                            'Редактирование, создание, удаление пользователей',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'role_read',
                            'Просмотр справочника ролей',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'role_manage',
                            'Редактирование, создание, удаление ролей',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                END $$
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}

do $$
declare
    client_id integer;
    resource_id integer;
begin
    -- Added scopes
    insert into api_scope (id, enabled, name, display_name, description, required, emphasize, show_in_discovery_document)
    values
        (default, true, 'admin_panel_api', 'Admin panel API scope', null, true, false, true),
        (default, true, 'notification_api', 'Notification API scope', null, true, false, true);

    -- Admin panel resource
    insert into api_resource (id, enabled, name, display_name, description, allowed_access_token_signing_algorithms, show_in_discovery_document, created, updated, last_accessed, non_editable)
    values (default, true, 'admin_panel_api', 'Admin panel resource', null, null, true, now(), null, null, false)
    RETURNING "id" INTO resource_id;

    insert into api_resource_scope (id, scope, api_resource_id) values (default, 'openid', resource_id);
    insert into api_resource_scope (id, scope, api_resource_id) values (default, 'admin_panel_api', resource_id);

    -- Notification resource
    insert into api_resource (id, enabled, name, display_name, description, allowed_access_token_signing_algorithms, show_in_discovery_document, created, updated, last_accessed, non_editable)
    values (default, true, 'notification_api', 'Notification API resource', null, null, true, now(), null, null, false)
    RETURNING "id" INTO resource_id;

    insert into api_resource_scope (id, scope, api_resource_id) values (default, 'openid', resource_id);
    insert into api_resource_scope (id, scope, api_resource_id) values (default, 'notification_api', resource_id);

    -- Update swagger client
    -- secret: swagger_secret
    select id into client_id from client where client.client_id = 'swagger';

    -- Added client scopes 
    insert into client_scope (id, scope, client_id)
    values
        (default, 'admin_panel_api', client_id),
        (default, 'notification_api', client_id);

    -- Added redirect uris
    -- Notification swagger uri
    insert into client_redirect_uri (redirect_uri, client_id)
    values ('http://localhost:5120/swagger/oauth2-redirect.html', client_id);

    -- Added admin panel client
    -- secret: admin_secret_rrp_240913
    insert into client (id, enabled, client_id, protocol_type, require_client_secret, require_consent, allow_remember_consent,
                        always_include_user_claims_in_id_token, require_pkce, allow_plain_text_pkce,
                        require_request_object, allow_access_tokens_via_browser,
                        front_channel_logout_session_required,
                        back_channel_logout_session_required, allow_offline_access, identity_token_lifetime, access_token_lifetime,
                        authorization_code_lifetime, absolute_refresh_token_lifetime,
                        sliding_refresh_token_lifetime, refresh_token_usage, update_access_token_claims_on_refresh,
                        refresh_token_expiration, access_token_type, enable_local_login, include_jwt_id,
                        always_send_client_claims, client_claims_prefix, created, device_code_lifetime, non_editable)
    values (default,true, 'admin_panel' ,'oidc', true, false,true,false,false,false,false,true,true,true,true,300,1800,300,2592000,1296000,0,false,0,0,true,true,false,'client_',current_timestamp,300,true)
    RETURNING id INTO client_id;

    insert into client_grant_type (id, grant_type, client_id)
    values (default, 'authorization_code', client_id);

    insert into client_scope (id, scope, client_id)
    values 
        (default, 'openid', client_id),
        (default, 'identity_api', client_id),
        (default, 'admin_panel_api', client_id);

    insert into client_secret (id, client_id, "value", type, created)
    values (default, client_id, 'YdEcStaKo+D/zF2fKw+zp+Wl3qD7tdO4EDjOj5hlnp0=', 'SharedSecret', current_timestamp);
end $$

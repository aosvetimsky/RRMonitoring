using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddIdentity4Configuration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<DateTime>(
			name: "creation_date",
			table: "signing_key",
			type: "timestamp with time zone",
			nullable: false,
			defaultValueSql: "now()",
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldDefaultValueSql: "now()");

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "identity_resource_property",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "identity_resource_property",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "identity_resource_claim",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "identity_resource",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "identity_resource",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "identity_resource",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "identity_resource",
			type: "character varying(1000)",
			maxLength: 1000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "identity_resource",
			type: "timestamp with time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone");

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_secret",
			type: "character varying(4000)",
			maxLength: 4000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "client_secret",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "expiration",
			table: "client_secret",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "client_secret",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "client_secret",
			type: "timestamp with time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone");

		migrationBuilder.AlterColumn<string>(
			name: "scope",
			table: "client_scope",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "redirect_uri",
			table: "client_redirect_uri",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_property",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "client_property",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "post_logout_redirect_uri",
			table: "client_post_logout_redirect_uri",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "provider",
			table: "client_id_p_restriction",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "grant_type",
			table: "client_grant_type",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "origin",
			table: "client_cors_origin",
			type: "character varying(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_claim",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "client_claim",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "user_code_type",
			table: "client",
			type: "character varying(100)",
			maxLength: 100,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "client",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "protocol_type",
			table: "client",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "pair_wise_subject_salt",
			table: "client",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "logo_uri",
			table: "client",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "last_accessed",
			table: "client",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "front_channel_logout_uri",
			table: "client",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "client",
			type: "character varying(1000)",
			maxLength: 1000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "client",
			type: "timestamp with time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone");

		migrationBuilder.AlterColumn<string>(
			name: "client_uri",
			table: "client",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "client_name",
			table: "client",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "client_id",
			table: "client",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "client_claims_prefix",
			table: "client",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "back_channel_logout_uri",
			table: "client",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "allowed_identity_token_signing_algorithms",
			table: "client",
			type: "character varying(100)",
			maxLength: 100,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_scope_property",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "api_scope_property",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_scope_claim",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "api_scope",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "api_scope",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_scope",
			type: "character varying(1000)",
			maxLength: 1000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_resource_secret",
			type: "character varying(4000)",
			maxLength: 4000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_resource_secret",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "expiration",
			table: "api_resource_secret",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_resource_secret",
			type: "character varying(1000)",
			maxLength: 1000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "api_resource_secret",
			type: "timestamp with time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone");

		migrationBuilder.AlterColumn<string>(
			name: "scope",
			table: "api_resource_scope",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_resource_property",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "api_resource_property",
			type: "character varying(250)",
			maxLength: 250,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_resource_claim",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "api_resource",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "api_resource",
			type: "character varying(200)",
			maxLength: 200,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "last_accessed",
			table: "api_resource",
			type: "timestamp with time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "api_resource",
			type: "character varying(200)",
			maxLength: 200,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_resource",
			type: "character varying(1000)",
			maxLength: 1000,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "api_resource",
			type: "timestamp with time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp without time zone");

		migrationBuilder.AlterColumn<string>(
			name: "allowed_access_token_signing_algorithms",
			table: "api_resource",
			type: "character varying(100)",
			maxLength: 100,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_permission_group_name",
			table: "permission_group",
			column: "name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_identity_resource_name",
			table: "identity_resource",
			column: "name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_client_client_id",
			table: "client",
			column: "client_id",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_api_scope_name",
			table: "api_scope",
			column: "name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_api_resource_name",
			table: "api_resource",
			column: "name",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_permission_group_name",
			table: "permission_group");

		migrationBuilder.DropIndex(
			name: "ix_identity_resource_name",
			table: "identity_resource");

		migrationBuilder.DropIndex(
			name: "ix_client_client_id",
			table: "client");

		migrationBuilder.DropIndex(
			name: "ix_api_scope_name",
			table: "api_scope");

		migrationBuilder.DropIndex(
			name: "ix_api_resource_name",
			table: "api_resource");

		migrationBuilder.AlterColumn<DateTime>(
			name: "creation_date",
			table: "signing_key",
			type: "timestamp without time zone",
			nullable: false,
			defaultValueSql: "now()",
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldDefaultValueSql: "now()");

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "identity_resource_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "identity_resource_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "identity_resource_claim",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "identity_resource",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "identity_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "identity_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "identity_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(1000)",
			oldMaxLength: 1000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "identity_resource",
			type: "timestamp without time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone");

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(4000)",
			oldMaxLength: 4000);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "client_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<DateTime>(
			name: "expiration",
			table: "client_secret",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "client_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "client_secret",
			type: "timestamp without time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone");

		migrationBuilder.AlterColumn<string>(
			name: "scope",
			table: "client_scope",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "redirect_uri",
			table: "client_redirect_uri",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "client_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "post_logout_redirect_uri",
			table: "client_post_logout_redirect_uri",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "provider",
			table: "client_id_p_restriction",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "grant_type",
			table: "client_grant_type",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "origin",
			table: "client_cors_origin",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(150)",
			oldMaxLength: 150);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "client_claim",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "client_claim",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "user_code_type",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(100)",
			oldMaxLength: 100,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "client",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "protocol_type",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "pair_wise_subject_salt",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "logo_uri",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "last_accessed",
			table: "client",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "front_channel_logout_uri",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(1000)",
			oldMaxLength: 1000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "client",
			type: "timestamp without time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone");

		migrationBuilder.AlterColumn<string>(
			name: "client_uri",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "client_name",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "client_id",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "client_claims_prefix",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "back_channel_logout_uri",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "allowed_identity_token_signing_algorithms",
			table: "client",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(100)",
			oldMaxLength: 100,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_scope_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "api_scope_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_scope_claim",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "api_scope",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "api_scope",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_scope",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(1000)",
			oldMaxLength: 1000,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_resource_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(4000)",
			oldMaxLength: 4000);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_resource_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<DateTime>(
			name: "expiration",
			table: "api_resource_secret",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_resource_secret",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(1000)",
			oldMaxLength: 1000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "api_resource_secret",
			type: "timestamp without time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone");

		migrationBuilder.AlterColumn<string>(
			name: "scope",
			table: "api_resource_scope",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<string>(
			name: "value",
			table: "api_resource_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(2000)",
			oldMaxLength: 2000);

		migrationBuilder.AlterColumn<string>(
			name: "key",
			table: "api_resource_property",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(250)",
			oldMaxLength: 250);

		migrationBuilder.AlterColumn<string>(
			name: "type",
			table: "api_resource_claim",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<DateTime>(
			name: "updated",
			table: "api_resource",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "api_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200);

		migrationBuilder.AlterColumn<DateTime>(
			name: "last_accessed",
			table: "api_resource",
			type: "timestamp without time zone",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "display_name",
			table: "api_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(200)",
			oldMaxLength: 200,
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "description",
			table: "api_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(1000)",
			oldMaxLength: 1000,
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created",
			table: "api_resource",
			type: "timestamp without time zone",
			nullable: false,
			oldClrType: typeof(DateTime),
			oldType: "timestamp with time zone");

		migrationBuilder.AlterColumn<string>(
			name: "allowed_access_token_signing_algorithms",
			table: "api_resource",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(100)",
			oldMaxLength: 100,
			oldNullable: true);
	}
}

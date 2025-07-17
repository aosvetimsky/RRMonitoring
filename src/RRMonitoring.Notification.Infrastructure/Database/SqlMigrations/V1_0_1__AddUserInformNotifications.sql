do $$
declare
    notification_group_id int;
    existing_notification_id uuid;
    notification_channel_email_id smallint;
    notification_channel_sms_id smallint;
    notification_identifier text;
	group_name text;
	template_subject text;
    template_data text;
begin

	select 'System' into group_name;

	if exists (select * from notification_group where name = group_name)
	then
		select id into notification_group_id from notification_group
		where name = group_name;
	else
		insert into notification_group (id, name)
		values (1, group_name)
		returning id into notification_group_id;
	end if;

    -- Notification channels
    select id into notification_channel_email_id from channel where name = 'Email';
    select id into notification_channel_sms_id from channel where name = 'Sms';

    -- Sms notification about successful changed phone number

	select 'UserPhoneChangedNotification' into notification_identifier;
	select 'Ваш номер телефона был изменен' into template_subject;
	select 'Ваш номер телефона был изменен на RedRock Pool, {{change_date}}' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_sms_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_sms_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_sms_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Email notification about successful changed phone number

	select 'UserPhoneChangedNotification' into notification_identifier;
	select 'Ваш номер телефона был изменен' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Ваш телефон был успешно изменён {{change_date}}.</div><div style="padding:16px 0;color:#000">В целях обеспечения безопасности Вашей учетной записи вывод средств, распределение доходов, изменение адреса автоматического вывода и настроек распределения доходов запрещены в течение 24 часа.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_email_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_email_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_email_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Email notification of account login

	select 'UserLoginNotification' into notification_identifier;
	select 'В ваш аккаунт был произведен вход' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">В ваш аккаунты был произведен вход {{login_date}}.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_email_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_email_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_email_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Email notification of account locking

	select 'UserLockoutNotification' into notification_identifier;
	select 'Ваш аккаунт временно заблокирован' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Ваш аккаунт был временно заблокирован из-за превышения допустимого количества попыток входа. В целях безопасности мы ограничили доступ к вашему аккаунту до {{end_date_lockout}}.</div><div style="padding:16px 0;color:#000">Пожалуйста, подождите указанное время, прежде чем снова пытаться войти в систему. Если вы забыли свои учетные данные, вы можете воспользоваться функцией восстановления пароля.</div><div style="padding:16px 0;color:#000">Если вы не совершали этих попыток входа, рекомендуем незамедлительно сменить пароль и обратиться в службу поддержки для дополнительной защиты вашего аккаунта.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_email_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_email_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_email_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Email notification of Two Factor state changed

	select 'UserUpdateTwoFactorStateNotification' into notification_identifier;
	select 'Уведомление об изменение настроек двухфакторной аутентификации' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">На вашем аккаунте была {{action}} двухфакторная аутентификация, {{change_date}}.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_email_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_email_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_email_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Sms notification of Two Factor state changed

	select 'UserUpdateTwoFactorStateNotification' into notification_identifier;
	select 'Уведомление об изменение настроек двухфакторной аутентификации' into template_subject;
	select 'На вашем аккаунте RedRock Pool была {{action}} двухфакторная аутентификация, {{change_date}}.' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_sms_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_sms_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_sms_id, existing_notification_id, template_data, template_subject);
	end if;

    -- Email notification of Authenticator state changed

	select 'UserUpdateAuthenticatorStateNotification' into notification_identifier;
	select 'Уведомление об изменение настроек аутентификатора' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">На вашем аккаунте был {{action}} аутентификатор, {{change_date}}.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_email_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_email_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_email_id, existing_notification_id, template_data, template_subject);
	end if;

	-- Sms notification of Authenticator state changed

	select 'UserUpdateAuthenticatorStateNotification' into notification_identifier;
	select 'Уведомление об изменение настроек аутентификатора' into template_subject;
	select 'На вашем аккаунте RedRock Pool был {{action}} аутентификатор, {{change_date}}.' into template_data;

	if exists (select * from notification where identifier = notification_identifier and group_id = notification_group_id)
	then
		select id into existing_notification_id from notification
		where identifier = notification_identifier and group_id = notification_group_id;
	else
		insert into notification (id, identifier, description, group_id)
		values (uuid_generate_v4(), notification_identifier, template_subject, notification_group_id)
		returning id into existing_notification_id;
	end if;

	if exists (select * from template where notification_id = existing_notification_id and channel_id = notification_channel_sms_id)
	then
		update template set data = template_data, subject = template_subject
		where notification_id = existing_notification_id and channel_id = notification_channel_sms_id;
	else
		insert into template (id, channel_id, notification_id, data, subject)
		values (uuid_generate_v4(), notification_channel_sms_id, existing_notification_id, template_data, template_subject);
	end if;

end $$

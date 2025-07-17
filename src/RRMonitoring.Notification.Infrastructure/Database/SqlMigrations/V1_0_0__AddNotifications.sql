create extension if not exists "uuid-ossp";

do $$
declare
    notification_group_id int;
    existing_notification_id uuid;
    notification_channel_email_id smallint;
    notification_channel_sms_id smallint;
    group_name text;
    notification_identifier text;
    template_subject text;
    template_data text;
begin
    -- System notification group
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

    -- Email notification for user registration
    select 'UserRegistrationNotification' into notification_identifier;
    select 'Регистрация пользователя' into template_subject;
    select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Для завершения регистрации, перейдите по ссылке ниже для подтверждения правильности введенного адреса электронной почты.</div><div style="padding:16px 0"><a href="{{link}}" style="font-size:16px;color:#15c">{{link}}</a></div><div style="padding:16px 0;color:#8e97b4">Если вы не регистрировались в RedRock Pool, проигнорируйте это письмо.</div></td></tr></table></td></tr></table></body></html>' into template_data;
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

    -- Sms notification for two-factor authentication
    select 'TwoFactorNotification' into notification_identifier;
    select 'Двухфакторная авторизация' into template_subject;
    select 'Код для авторизации на RedRock Pool: {{code}}' into template_data;
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

    -- Email notification for reset password
    select 'ResetPasswordNotification' into notification_identifier;
    select 'Сброс пароля' into template_subject;
    select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Вы хотите сбросить пароль. Если Вы не выполняли это действие, пожалуйста, немедленно измените пароль для входа.</div><div style="padding:16px 0;font-size:28px;font-weight:700;letter-spacing:3px;text-align:center;color:#000">{{code}}</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

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

    -- Sms notification for reset password
    select 'ResetPasswordNotification' into notification_identifier;
    select 'Сброс пароля' into template_subject;
    select 'Код для смены пароля на RedRock Pool: {{code}}' into template_data;

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

    -- Email notification for reset password
    select 'ResetPasswordEmailNotification' into notification_identifier;
    select 'Сброс пароля' into template_subject;
    select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Вы хотите Сбросить пароль. Для сброса пароля перейдите по ссылке ниже. Если Вы не выполняли это действие, пожалуйста, немедленно измените пароль для входа.</div><div style="padding:16px 0"><a href="{{link}}" style="font-size:16px;color:#15c">{{link}}</a></div><div style="padding:16px 0;color:#8e97b4">Если вы не регистрировались в RedRock Pool, проигнорируйте это письмо.</div></td></tr></table></td></tr></table></body></html>' into template_data;

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

    -- Email notification for change email (to new email)
    select 'UserConfirmEmailChangeNotification' into notification_identifier;
    select 'Смена электронной почты' into template_subject;
    select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Вы хотите изменить адрес электронной почты, для подтверждения используйте код. Если Вы не выполняли это действие, пожалуйста, немедленно измените пароль для входа.</div><div style="padding:16px 0;font-size:28px;font-weight:700;letter-spacing:3px;text-align:center;color:#000">{{code}}</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

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

	-- Email notification of update email to the old email

	select 'UserUpdateEmailNotification' into notification_identifier;
	select 'Уведомление о смене электронной почты' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Ваш email адрес успешно изменен, на новый Email: {{new_email}}.</div><div style="padding:16px 0;color:#000">В целях обеспечения безопасности Вашей учетной записи вывод средств, распределение доходов, изменение адреса автоматического вывода и настроек распределения доходов запрещены в течение 24 часа.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

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

	-- Sms notification of email changed

	select 'UserUpdateEmailNotification' into notification_identifier;
	select 'Уведомление о смене электронной почты' into template_subject;
	select 'Ваш электронная почта была изменена на RedRock Pool, {{new_email}}' into template_data;

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

	-- Send sms to phone number change (new phone number)

    select 'UserConfirmPhoneChangeSmsNotification' into notification_identifier;
    select 'Смена номера телефона' into template_subject;
    select 'Код для смены номера телефона на RedRock Pool: {{code}}' into template_data;

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

	-- Send email about successful password change

	select 'UserPasswordChangedNotification' into notification_identifier;
	select 'Уведомление об изменившемся пароле' into template_subject;
	select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Ваш пароль был успешно изменён, {{change_date}}.</div><div style="padding:16px 0;color:#000">В целях обеспечения безопасности Вашей учетной записи вывод средств, распределение доходов, изменение адреса автоматического вывода и настроек распределения доходов запрещены в течение 24 часа.</div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>' into template_data;

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

	select 'UserPasswordChangedNotification' into notification_identifier;
	select 'Уведомление об изменившемся пароле' into template_subject;
	select 'На вашем аккаунте в RedRock Pool был изменён пароль, {{change_date}}' into template_data;

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

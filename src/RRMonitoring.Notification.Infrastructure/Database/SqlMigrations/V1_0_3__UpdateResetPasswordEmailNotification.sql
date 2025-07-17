do $$
declare
	v_notification_id uuid;
    v_new_template_data text;
begin
select '<!DOCTYPE html><html lang="ru"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>RedRock Pool</title></head><body style="margin:0;padding:0;font-family:Arial,sans-serif;font-size:18px;font-weight:400;line-height:1.5;color:#000"><table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td align="center" style="padding:16px 8px"><table width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width:540px"><tr><td class="header" style="padding:32px 48px;background:#fff linear-gradient(120deg,transparent 32%,#f36126 32%,#f36126 68%,transparent 68%) top center repeat-x;background-size:30px 20px;border:1px solid #f9f9f9"><div style="padding:16px 0"><img src="https://wrlimit.com/redrock/email-logo.png" alt="RedRock Pool"></div><div style="padding:16px 0;font-size:24px;font-weight:700;color:#000">Здравствуйте, {{username}}</div><div style="padding:16px 0;color:#000">Вы хотите Сбросить пароль. Для сброса пароля перейдите по ссылке ниже. Если Вы не выполняли это действие, пожалуйста, немедленно измените пароль для входа.</div><div style="padding:16px 0"><a href="{{link}}" style="font-size:16px;color:#15c">{{link}}</a></div><div style="padding:16px 0;color:#8e97b4">Это письмо сформировано автоматически, не отвечайте на него.</div></td></tr></table></td></tr></table></body></html>'
into v_new_template_data;

select id into v_notification_id
from notification
where identifier = 'ResetPasswordEmailNotification';

if v_notification_id is not null then
update template
set data = v_new_template_data
where template.notification_id = v_notification_id;
end if;

end $$;

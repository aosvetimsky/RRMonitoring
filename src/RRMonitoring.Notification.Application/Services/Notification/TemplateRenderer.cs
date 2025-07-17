using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scriban;
using Scriban.Runtime;

namespace RRMonitoring.Notification.Application.Services.Notification;

public static class TemplateRenderer
{
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
	public static ValueTask<string> Render(string templateText, string jsonData)
	{
		try
		{
			var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonData);
			var scriptObject = BuildScriptObject(expandoObject);

			var templateContext = new TemplateContext();
			templateContext.PushGlobal(scriptObject);

			var template = Template.Parse(templateText);

			return template.RenderAsync(templateContext);
		}
		catch (Exception)
		{
			return ValueTask.FromResult(templateText);
		}
	}

	private static ScriptObject BuildScriptObject(ExpandoObject expando)
	{
		var dict = (IDictionary<string, object>)expando;

		var scriptObject = new ScriptObject();

		foreach (var kv in dict)
		{
			var renamedKey = StandardMemberRenamer.Rename(kv.Key);

			if (kv.Value is ExpandoObject expandoValue)
			{
				scriptObject.Add(renamedKey, BuildScriptObject(expandoValue));
			}
			else
			{
				scriptObject.Add(renamedKey, kv.Value);
			}
		}

		return scriptObject;
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public static class TemplateHelper
	{
		public static string GetScriptTemplate(Type type)
		{
			var thisType = typeof(TemplateHelper);
			using (var stream = thisType.Assembly.GetManifestResourceStream($"{thisType.Namespace}.ScriptTemplate.cs"))
			{
				using (var reader = new StreamReader(stream))
				{
					return string.Format(CultureInfo.InvariantCulture, type.Namespace, type.Name);
				}
			}
		}
	}
}

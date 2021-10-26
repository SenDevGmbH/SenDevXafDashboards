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
			if (type is null)
				throw new ArgumentNullException(nameof(type));

			var thisType = typeof(TemplateHelper);
			using (var stream = thisType.Assembly.GetManifestResourceStream($"{thisType.Namespace}.ScriptTemplate.template"))
			{
				return GetScriptTemplate(type, stream);
			}
		}

		public static string GetScriptTemplate(Type type, Stream templateContentStream)
		{
			if (type is null)
				throw new ArgumentNullException(nameof(type));

			if (templateContentStream is null)
				throw new ArgumentNullException(nameof(templateContentStream));

			using (var reader = new StreamReader(templateContentStream))
			{
				return reader.ReadToEnd().Replace("|namespace|", type.Namespace).Replace("|classname|", type.Name);
			}
		}
	}
}

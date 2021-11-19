using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public static class AssembliesHelper
	{
		public static string GetNeststandardAssemblyPath()
		{
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetName().Name == "netstandard")?.Location;
		}

		public static string[] GetReferencedAssembliesPaths(XafApplication application)
		{
			var assembly = application.GetType().Assembly;
			var assemmblyNames =
				application.TypesInfo.PersistentTypes
				.AsEnumerable()
				.SelectMany(ti => GetTypesHierarchy(ti.Type)).Select(t => t.Assembly)
				.Where(a => a.GetName().Name != "mscorlib")
				.Select(a => a.Location)
				.ToList();

			var netstandardPath = GetNeststandardAssemblyPath();
			if (!string.IsNullOrWhiteSpace(netstandardPath))
				assemmblyNames.Add(netstandardPath);

			var assemblies = new HashSet<string>(assemmblyNames, StringComparer.OrdinalIgnoreCase);

			var module = application.Modules.FindModule<SenDevDashboardsModule>();
			if (module != null)
			{
				if (module.ScriptReferenceAssemblies != null)
				{
					foreach (var ra in module.ScriptReferenceAssemblies)
						assemblies.Add(ra);
				}

				if (module.DefaultBusinessObjectType != null)
				{
					assemblies.Add(module.DefaultBusinessObjectType.Assembly.Location);
				}
			}

			return assemblies.ToArray();
		}

		private static IEnumerable<Type> GetTypesHierarchy(Type type)
		{
			for (var currentType = type; currentType != null; currentType = currentType.BaseType)
			{
				yield return currentType;
			}

			foreach (var itf in type.GetInterfaces().SelectMany(GetTypesHierarchy))
			{
				yield return itf;
			}
		}
	}
}

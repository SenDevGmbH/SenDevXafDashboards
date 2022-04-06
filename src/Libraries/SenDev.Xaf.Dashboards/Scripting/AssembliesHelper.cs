using System;
using System.Collections.Generic;
using System.IO;
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
			var assemblyNames =
				application.TypesInfo.PersistentTypes
				.AsEnumerable()
				.SelectMany(ti => GetTypesHierarchy(ti.Type)).Select(t => t.Assembly)
				.Where(a => a.GetName().Name != "mscorlib")
				.Select(a => a.Location)
				.ToList();

			var netstandardPath = GetNeststandardAssemblyPath();
			if (!string.IsNullOrWhiteSpace(netstandardPath))
				assemblyNames.Add(netstandardPath);

			var systemRuntimePath = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll");
			if (File.Exists(systemRuntimePath))
				assemblyNames.Add(systemRuntimePath);

			assemblyNames.Add(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location);
			assemblyNames.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location);
			assemblyNames.Add(typeof(IQueryable<>).Assembly.Location);
          
			var assemblies = new HashSet<string>(assemblyNames, StringComparer.OrdinalIgnoreCase);

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

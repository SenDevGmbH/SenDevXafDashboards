using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ScriptDataSource
	{
		public ScriptDataSource(string script)
		{
			Script = script;
		}

		public XafApplication Application
		{
			get; set;
		}

		public string Script
		{
			get;
		}

		public object GetData(IDictionary<string, object> parameters)
		{
			var data = GetDataCore(parameters, out var objectSpace);
			if (data is IDataReader reader)
				return new DataReaderList(reader);

			return new ScriptResultList(data, objectSpace.TypesInfo);

		}

		private IEnumerable<Type> GetTypesHierarchy(Type type)
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


		private string GetAssemblyLocation(Assembly assembly)
		{
			return assembly.Location;
		}

		private string GetNeststandardAssemblyPath()
		{
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetName().Name == "netstandard")?.Location;
		}
		protected virtual string[] GetReferencedAssembliesPaths()
		{
			var assembly = GetType().Assembly;
			var assemmblyNames =
				Application.TypesInfo.PersistentTypes
				.AsEnumerable()
				.SelectMany(ti => GetTypesHierarchy(ti.Type)).Select(t => t.Assembly)
				.Where(a => a.GetName().Name != "mscorlib")
				.Select(GetAssemblyLocation)
				.ToList();

			var netstandardPath = GetNeststandardAssemblyPath();
			if (!string.IsNullOrWhiteSpace(netstandardPath))
				assemmblyNames.Add(netstandardPath);

			var assemblies = new HashSet<string>(assemmblyNames, StringComparer.OrdinalIgnoreCase);

			var module = Application.Modules.FindModule<SenDevDashboardsModule>();
			if (module != null)
			{
				if (module.ScriptReferenceAssemblies != null)
				{
					foreach (var ra in module.ScriptReferenceAssemblies)
						assemblies.Add(ra);
				}

				if (module.DefaultBusinessObjectType != null)
				{
					assemblies.Add(GetAssemblyLocation(module.DefaultBusinessObjectType.Assembly));
				}
			}

			return assemblies.ToArray();
		}

		private object GetDataCore(IDictionary<string, object> parameters, out XPObjectSpace objectSpace)
		{
			objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
			var context = new ScriptContext(objectSpace, parameters);
			var compilationHelper = new ScriptCompilationHelper(GetReferencedAssembliesPaths());
			dynamic scriptObject = compilationHelper.CreateObject(Script);
			return scriptObject.GetData(context);
		}


		private static bool IsGenericCollection(object obj)
		{
			if (obj == null)
				return false;
			var type = obj.GetType();
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
			
		}
		public object GetDataForDataExtract()
		{
			var data = GetDataCore(new Dictionary<string, object>(), out var os);
			if (data is byte[])
				return data;
			
			if (data is IDataReader reader)
				return new DataReaderList(reader);

			if (data is ICollection || IsGenericCollection(data))
				return data;

			return new DashboardDataList(() =>
				{
					var queryable = (IQueryable)GetDataCore(new Dictionary<string, object>(), out var objectSpace);
					return (queryable, objectSpace);
				}, 1);
		}

	}

}


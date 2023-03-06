using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Xpo;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public interface IScriptDataSource
	{
		string Script
		{
			get;
		}

		object GetData(IDictionary<string, object> parameters);
	}
	public abstract class ScriptDataSourceBase : IScriptDataSource
	{

		protected ScriptDataSourceBase(string script)
		{
			Script = script;
		}

		public string Script
		{
			get;
		}

		public object GetData(IDictionary<string, object> parameters)
		{
			var data = GetDataCore(parameters);
			if (data is IDataReader reader)
				return new DataReaderList(reader);

			return new ScriptResultList(data, GetTypesInfo());

		}

		protected abstract ITypesInfo GetTypesInfo();
		protected abstract object GetDataCore(IDictionary<string, object> parameters);
	}
	public class ScriptDataSource  : ScriptDataSourceBase
	{
		public ScriptDataSource(string script) : base(script)
		{
		}

		public XafApplication Application
		{
			get; set;
		}

		protected override object GetDataCore(IDictionary<string, object> parameters)
		{
			var objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
			var context = new ObjectSpaceScriptContext(objectSpace, parameters);
			var compilationHelper = new ScriptCompilationHelper(AssembliesHelper.GetReferencedAssembliesPaths(Application));
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
			var data = GetDataCore(new Dictionary<string, object>());
			if (data is byte[])
				return data;
			
			if (data is IDataReader reader)
				return new DataReaderList(reader);

			if (data is ICollection || IsGenericCollection(data))
				return data;

			return new DashboardDataList(() =>
				{
					var queryable = (IQueryable)GetDataCore(new Dictionary<string, object>());
					return (queryable, objectSpace);
				}, 1);
		}

	}

}


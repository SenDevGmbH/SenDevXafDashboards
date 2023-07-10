﻿using DevExpress.ExpressApp;
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


		private object GetDataCore(IDictionary<string, object> parameters, out XPObjectSpace objectSpace)
		{
			objectSpace = (XPObjectSpace)Application.CreateObjectSpace();
			var context = new ScriptContext(objectSpace, parameters);
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
			var data = GetDataCore(new Dictionary<string, object>(), out var os);
			if (data == null || data is IEnumerable enumerable && !enumerable.Cast<object>().Any())
				return null;
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


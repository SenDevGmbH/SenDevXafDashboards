using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class ScriptContext
	{
		public ScriptContext(IObjectSpace objectSpace, IDictionary<string, object> parameters)
		{
			ObjectSpace = objectSpace;
			Parameters = parameters;
		}

		public IObjectSpace ObjectSpace
		{
			get;
		}

		public IDictionary<string, object> Parameters
		{
			get;
		}
		public IQueryable<T> Query<T>() => ObjectSpace.GetObjectsQuery<T>();

		public Session Session => ((XPObjectSpace)ObjectSpace).Session;
	}
}

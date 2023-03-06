using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class ObjectSpaceScriptContext : ScriptContextBase
	{
		public ObjectSpaceScriptContext(IObjectSpace objectSpace, IDictionary<string, object> parameters) : base(parameters)
		{
			ObjectSpace = objectSpace;
		}

		public IObjectSpace ObjectSpace
		{
			get;
		}

		public override IQueryable<T> Query<T>() => ObjectSpace.GetObjectsQuery<T>();

		public Session Session => ((XPObjectSpace)ObjectSpace).Session;
	}
}

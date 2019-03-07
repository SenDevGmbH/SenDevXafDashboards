using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class ScriptContext
	{
		public ScriptContext(IObjectSpace objectSpace)
		{
			ObjectSpace = objectSpace;
		}

		public IObjectSpace ObjectSpace
		{
			get;
		}

		public IQueryable<T> Query<T>() => ObjectSpace.GetObjectsQuery<T>();

		public Session Session => ((XPObjectSpace)ObjectSpace).Session;
	}
}

using System.Collections.Generic;
using System.Linq;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public abstract class ScriptContextBase
	{
		public ScriptContextBase(IDictionary<string, object> parameters)
		{
			Parameters = parameters;

		}
		public IDictionary<string, object> Parameters
		{
			get;
		}

		public abstract IQueryable<T> Query<T>();
	}
}

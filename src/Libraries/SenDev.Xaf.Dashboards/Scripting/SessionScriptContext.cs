using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{
	public class SessionScriptContext : ScriptContextBase
	{
		public SessionScriptContext(Session session, IDictionary<string, object> parameters) : base(parameters)
		{
			Session = session;
		}

		private Session Session
		{
			get;
		}

		public override IQueryable<T> Query<T>() => Session.Query<T>();
	}
}

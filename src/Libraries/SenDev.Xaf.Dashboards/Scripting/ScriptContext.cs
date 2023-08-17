using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class ScriptContext
	{

		[Obsolete]
		public ScriptContext(IObjectSpace objectSpace, IDictionary<string, object> parameters)
		{
			ObjectSpace = objectSpace;
			Parameters = parameters;
		}

		public ScriptContext(Func<IObjectSpace> createObjectSpaceFunc, IDictionary<string, object> parameters, CancellationToken cancellationToken)
		{
			CreateObjectSpaceFunc = createObjectSpaceFunc ?? throw new ArgumentNullException(nameof(createObjectSpaceFunc));
			CancellationToken = cancellationToken;
			Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
			ObjectSpace = CreateObjectSpace();
		}

		public IObjectSpace CreateObjectSpace() => CreateObjectSpaceFunc();

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

		public Func<IObjectSpace> CreateObjectSpaceFunc
		{
			get;
		}
		public CancellationToken CancellationToken
		{
			get;
		}
	}
}

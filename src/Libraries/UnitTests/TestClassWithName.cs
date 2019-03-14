using System.Collections.Generic;
using System.Linq;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{

	public class TestClassWithName : BaseObject
	{
		public TestClassWithName(Session session) : base(session)
		{
		}
		private string name;
		public string Name
		{
			get => name;
			set => SetPropertyValue(nameof(Name), ref name, value);
		}

	}
}

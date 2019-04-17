using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace UnitTests.TestClasses
{
	public class TestClassWithReference : BaseObject
	{
		public TestClassWithReference(Session session) : base(session)
		{
		}


		private TestClassWithName namedObject;
		public TestClassWithName NamedObject
		{
			get => namedObject;
			set => SetPropertyValue(nameof(NamedObject), ref namedObject, value);
		}


		private string title;
		public string Title
		{
			get => title;
			set => SetPropertyValue(nameof(Title), ref title, value);
		}
	}
}

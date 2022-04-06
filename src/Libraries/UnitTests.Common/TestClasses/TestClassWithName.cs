using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

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

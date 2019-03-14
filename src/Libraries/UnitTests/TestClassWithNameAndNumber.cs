using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace UnitTests
{
	public class TestClassWithNameAndNumber : BaseObject
	{
		public TestClassWithNameAndNumber(Session session) : base(session)
		{
		}


		private string name;
		public string Name
		{
			get => name;
			set => SetPropertyValue(nameof(Name), ref name, value);
		}


		private int sequentialNuber;
		public int SequentialNumber
		{
			get => sequentialNuber;
			set => SetPropertyValue(nameof(SequentialNumber), ref sequentialNuber, value);
		}
	}
}

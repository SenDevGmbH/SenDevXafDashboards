using System.Collections.Generic;
using System.Linq;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SenDev.Xaf.Dashboards.Scripting;
using Xunit;

namespace UnitTests
{
	public partial class DashboardDataListTests : XpoTestBase
	{

		[Fact]
		public void TestEnumerator()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			{

				var numbers = new List<int>();
				using (var objectSpace = application.CreateObjectSpace())
				{
					for (int i = 1; i <= 10000; i++)
					{
						TestClassWithNameAndNumber tt = objectSpace.CreateObject<TestClassWithNameAndNumber>();

						tt.SequentialNumber = i;
						tt.Name = $"TaskType {i}";
						numbers.Add(i);
					}
					objectSpace.CommitChanges();
				}

				using (var objectSpace = application.CreateObjectSpace())
				{
					var list = new DashboardDataList(() => (objectSpace.GetObjectsQuery<TestClassWithNameAndNumber>().OrderBy(l => l.SequentialNumber), objectSpace), 1);
					Assert.Equal(list.Cast<TestClassWithNameAndNumber>().Select(l => l.SequentialNumber), numbers);
				}
			}
		}

	}
}

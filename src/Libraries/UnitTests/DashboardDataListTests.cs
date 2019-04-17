using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.Native;
using DevExpress.Data.Browsing;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SenDev.Xaf.Dashboards.Scripting;
using UnitTests.TestClasses;
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

		[Fact]
		public void ChainedPropertiesTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				for (int i = 1; i <= 10000; i++)
				{
					var objectWithReference = objectSpace.CreateObject<TestClassWithReference>();
					var objectWithName = objectSpace.CreateObject<TestClassWithName>();
					objectWithName.Name = $"Name {i}";
					objectWithReference.NamedObject = objectWithName;
					objectWithReference.Title = $"Reference to {i}";
				}
				objectSpace.CommitChanges();

				var list = new ScriptResultList(objectSpace.GetObjectsQuery<TestClassWithReference>(), application.TypesInfo);

				PropertyDescriptorCollection dataListProperties = list.GetItemProperties(null);
				var complexProperty = dataListProperties.Find(nameof(TestClassWithReference.NamedObject), false);
				Assert.NotNull(complexProperty);
				var nestedProperties = list.GetItemProperties(new[] { complexProperty });
				Assert.Equal(2, nestedProperties.Count);

			}
		}
	}
}

using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Controllers;
using Xunit;

namespace UnitTests
{
	public class DashboardDataExtractControllerTests : XpoTestBase
	{

		[Fact]
		public void SaveInvalidScriptTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var collectionSource = new CollectionSource(objectSpace, typeof(DashboardDataExtract));
				var listView = new ListView(collectionSource, application, true);

				var controller = application.CreateController<DashboardDataExtractController>();
				controller.SetView(listView);

				controller.Active.Clear();
				controller.Active["Test"] = true;

				var dataExtract = objectSpace.CreateObject<DashboardDataExtract>();
				dataExtract.Script = "invalid script";

				var exception = Assert.Throws<UserFriendlyException>(() => objectSpace.CommitChanges());

				Assert.Empty(objectSpace.GetObjects<DashboardDataExtract>());
			}
		}

		[Fact]
		public void SaveValidScriptTest()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			using (var objectSpace = application.CreateObjectSpace())
			{
				var collectionSource = new CollectionSource(objectSpace, typeof(DashboardDataExtract));
				var listView = new ListView(collectionSource, application, true);

				var controller = application.CreateController<DashboardDataExtractController>();
				controller.SetView(listView);

				controller.Active.Clear();
				controller.Active["Test"] = true;

				var dataExtract = objectSpace.CreateObject<DashboardDataExtract>();
				dataExtract.Script = @"
					public class MyClass
					{
						public int MyMethod() { return 42; }
					}";
				objectSpace.CommitChanges();

				Assert.Single(objectSpace.GetObjects<DashboardDataExtract>());
			}
		}
	}
}

using SenDev.Xaf.Dashboards.Controllers;
using Xunit;

namespace UnitTests
{
	public class DashboardDataExtractControllerTests : XpoTestBase
	{
		[Fact]
		public void Test1()
		{
			using (var application = XpoInMemoryXafApplication.CreateInstance())
			{
				var controller = application.CreateController<DashboardDataExtractController>();
				
				Assert.NotNull(controller);
			}
		}
	}
}

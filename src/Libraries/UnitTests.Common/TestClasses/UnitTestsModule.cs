using DevExpress.ExpressApp;

namespace UnitTests
{


	public class UnitTestsModule : ModuleBase
	{
		public UnitTestsModule()
		{
			RequiredModuleTypes.Add(typeof(SenDev.Xaf.Dashboards.SenDevDashboardsModule));
		}
	}
}

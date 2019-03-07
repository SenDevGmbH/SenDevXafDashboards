using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{


	public class DashboardDesignerManagerEx : DashboardDesignerManager
	{
		public DashboardDesignerManagerEx(XafApplication application) : base(application)
		{
		}

		protected override DashboardDesignerCreatorBase CreateDashboardDesignerCreator()
		{
			//TODO: Implement also the toolbar
			return new RibbonDashboardDesignerCreatorEx(application);
		}
	}
}

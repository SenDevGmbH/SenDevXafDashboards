using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Win.DataSourceWizard;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class DashboardCustomizationController : ViewController
	{

		protected override void OnActivated()
		{
			base.OnActivated();
			var dashboardDesignerController = Frame.GetController<WinShowDashboardDesignerController>();
			dashboardDesignerController.DashboardDesignerManager = new DashboardDesignerManagerEx(Application);
		}
	}

}

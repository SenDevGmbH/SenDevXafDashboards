using DevExpress.DashboardCommon;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class CustomizeDashboardEventArgs
	{
		public CustomizeDashboardEventArgs(Dashboard dashboard, bool designMode)
		{
			Dashboard = dashboard;
			DesignMode = designMode;
		}

		public Dashboard Dashboard
		{
			get;
		}
		public bool DesignMode
		{
			get;
		}
	}
}

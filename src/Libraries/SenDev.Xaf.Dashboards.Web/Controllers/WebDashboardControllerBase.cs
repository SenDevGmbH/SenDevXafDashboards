using System;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public abstract class WebDashboardControllerBase : ObjectViewController<DetailView, IDashboardData>
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			var dashboardViewerViewItem = GetDashboardViewItem();
			if (dashboardViewerViewItem != null)
			{
				if (dashboardViewerViewItem.DashboardControl != null)
				{
					CustomizeDashboardControl(dashboardViewerViewItem.DashboardControl);
				}
				dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
			}
		}

		protected abstract void CustomizeDashboardControl(ASPxDashboard dashboard);

		private WebDashboardViewerViewItem GetDashboardViewItem()
		{
			return View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
		}

		private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
		{
			CustomizeDashboardControl(((WebDashboardViewerViewItem)sender).DashboardControl);
		}

		protected override void OnDeactivated()
		{
			var dashboardViewerViewItem = GetDashboardViewItem();
			if (dashboardViewerViewItem != null)
			{
				dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
			}
			base.OnDeactivated();
		}
	}

}


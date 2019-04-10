using System;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public abstract class WebCustomizeDashboardViewerControllerBase : ObjectViewController<ObjectView, IDashboardData>
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			var dashboardViewerItem = FindDashboardViewerItem();
			if (dashboardViewerItem != null)
			{
				if (dashboardViewerItem.DashboardControl != null)
				{
					CustomizeDashboardControl(dashboardViewerItem.DashboardControl);
				}
				dashboardViewerItem.ControlCreated += DashboardViewerViewItem_ControlCreated;

			}

		}

		protected WebDashboardViewerViewItem FindDashboardViewerItem() => View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;

		protected override void OnDeactivated()
		{
			var dashboardViewerItem = FindDashboardViewerItem();
			if (dashboardViewerItem != null)
			{
				dashboardViewerItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
			}
			base.OnDeactivated();
		}
		private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e) => CustomizeDashboardControl(((WebDashboardViewerViewItem)sender).DashboardControl);

		protected abstract void CustomizeDashboardControl(ASPxDashboard dashboardControl);

	}

}

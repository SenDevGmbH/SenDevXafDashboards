using System;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public abstract class WinCustomizeDashboardViewerControllerBase : ObjectViewController<ObjectView, IDashboardData>
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			var dashboardViewerItem = FindDashboardViewerItem();
			if (dashboardViewerItem != null)
			{
				if (dashboardViewerItem.Viewer != null)
				{
					CustomizeDashboardViewer(dashboardViewerItem.Viewer);
				}
				dashboardViewerItem.ControlCreated += DashboardViewerViewItem_ControlCreated;

			}

		}

		protected WinDashboardViewerViewItem FindDashboardViewerItem() => View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;

		protected override void OnDeactivated()
		{
			var dashboardViewerItem = FindDashboardViewerItem();
			if (dashboardViewerItem != null)
			{
				dashboardViewerItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
			}
			base.OnDeactivated();
		}
		private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e) => CustomizeDashboardViewer(((WinDashboardViewerViewItem)sender).Viewer);

		protected abstract void CustomizeDashboardViewer(DashboardViewer dashboardViewer);

	}

}

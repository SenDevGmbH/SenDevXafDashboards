using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.BaseImpl;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class WinDashboardDataSourceController : ObjectViewController<ObjectView, DashboardData>
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
			if (dashboardViewerViewItem != null)
			{
				if (dashboardViewerViewItem.Viewer != null)
				{
					CustomizeDashboardViewer(dashboardViewerViewItem.Viewer);
				}
				dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;

			}
			var showDesignerController = Frame.GetController<WinShowDashboardDesignerController>();
			showDesignerController.DashboardDesignerManager.DashboardDesignerCreated += DashboardDesignerManager_DashboardDesignerCreated;

		}

		private void DashboardDesignerManager_DashboardDesignerCreated(object sender, DashboardDesignerShownEventArgs e)
		{
			e.DashboardDesigner.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
		}


		private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
		{
			WinDashboardViewerViewItem dashboardViewerViewItem = sender as WinDashboardViewerViewItem;
			CustomizeDashboardViewer(dashboardViewerViewItem.Viewer);
		}
		private void CustomizeDashboardViewer(DashboardViewer dashboardViewer)
		{
			dashboardViewer.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
			dashboardViewer.AllowPrintDashboardItems = true;
		}

		private void DashboardViewer_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
		{
			if (e.ConnectionParameters is ExtractDataSourceConnectionParameters extractParameters)
			{
				if (Guid.TryParse(extractParameters.FileName, out var id))
				{
					var extract = ObjectSpace.GetObjectByKey<DashboardDataExtract>(id);
					if (extract != null)
						extract.ConfigureConnectionParameters(extractParameters);
				}
			}
		}



		protected override void OnDeactivated()
		{
			WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
			if (dashboardViewerViewItem != null)
			{
				dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
			}
			base.OnDeactivated();
		}
	}

}

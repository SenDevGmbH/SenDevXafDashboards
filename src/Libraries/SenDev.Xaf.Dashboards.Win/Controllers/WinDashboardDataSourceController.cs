using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.BaseImpl;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Controllers;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class WinDashboardDataSourceController : WinCustomizeDashboardViewerControllerBase
	{
		protected override void OnActivated()
		{
			base.OnActivated();

			var showDesignerController = Frame.GetController<WinShowDashboardDesignerController>();
			showDesignerController.DashboardDesignerManager.DashboardDesignerCreated += DashboardDesignerManager_DashboardDesignerCreated;

		}

		private void DashboardDesignerManager_DashboardDesignerCreated(object sender, DashboardDesignerShownEventArgs e)
		{
			e.DashboardDesigner.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
			e.DashboardDesigner.DashboardLoaded += DashboardDesigner_DashboardLoaded;
		}

		private void DashboardDesigner_DashboardLoaded(object sender, DashboardLoadedEventArgs e)
		{
			CustomizeDashboard(((DashboardDesigner)sender).Dashboard, true);

		}

		protected override void CustomizeDashboardViewer(DashboardViewer dashboardViewer)
		{
			dashboardViewer.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
			dashboardViewer.AllowPrintDashboardItems = true;
			dashboardViewer.DashboardLoaded += DashboardViewer_DashboardLoaded;
		}

		private void DashboardViewer_DashboardLoaded(object sender, DashboardLoadedEventArgs e)
		{
			CustomizeDashboard(((DashboardViewer)sender).Dashboard, false);
		}

		private void CustomizeDashboard(Dashboard dashboard, bool designMode)
		{
			var customizeDashboardController = Frame.GetController<CustomizeDashboardController>();
			if (customizeDashboardController != null && customizeDashboardController.ShouldCustomizeDashboard)
			{
				customizeDashboardController.RaiseCustomizeDashboard(new CustomizeDashboardEventArgs(dashboard, designMode));
			}
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




	}

}

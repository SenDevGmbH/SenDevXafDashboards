using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Controllers;
using SenDev.Xaf.Dashboards.Utils;

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

		private DashboardConnectionHelper CreateConnectionHelper() =>
			new DashboardConnectionHelper(Application, ObjectSpace);

		private void DashboardDesignerManager_DashboardDesignerCreated(object sender, DashboardDesignerShownEventArgs e)
		{
			e.DashboardDesigner.ConfigureDataConnection += DashboardDesigner_ConfigureDataConnection;
			e.DashboardDesigner.DashboardLoaded += DashboardDesigner_DashboardLoaded;
		}

		private void DashboardDesigner_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
		{
			var extract = CreateConnectionHelper().ConfigureDataConnection(e.ConnectionParameters);
			extract?.BackendType?.CreateBackend()?.PatchDashboard(((DashboardDesigner)sender).Dashboard, extract, designMode: true);
		}

		private void DashboardDesigner_DashboardLoaded(object sender, DashboardLoadedEventArgs e) =>
			CustomizeDashboard(((DashboardDesigner)sender).Dashboard, designMode: true);

		protected override void CustomizeDashboardViewer(DashboardViewer dashboardViewer)
		{
			dashboardViewer.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
			dashboardViewer.AllowPrintDashboardItems = true;
			dashboardViewer.DashboardLoaded += DashboardViewer_DashboardLoaded;
			dashboardViewer.PopulateUnusedDataSources = true;
		}

		private void DashboardViewer_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
		{
			var extract = CreateConnectionHelper().ConfigureDataConnection(e.ConnectionParameters);
			extract?.BackendType?.CreateBackend()?.PatchDashboard(((DashboardViewer)sender).Dashboard, extract, designMode: false);
		}

		private void DashboardViewer_DashboardLoaded(object sender, DashboardLoadedEventArgs e) =>
			CustomizeDashboard(((DashboardViewer)sender).Dashboard, designMode: false);

		private void CustomizeDashboard(Dashboard dashboard, bool designMode)
		{
			var customizeDashboardController = Frame.GetController<CustomizeDashboardController>();
			if (customizeDashboardController != null && customizeDashboardController.ShouldCustomizeDashboard)
				customizeDashboardController.RaiseCustomizeDashboard(new CustomizeDashboardEventArgs(dashboard, designMode));
		}
	}
}

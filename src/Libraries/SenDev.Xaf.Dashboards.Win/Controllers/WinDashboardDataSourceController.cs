using System.Collections.Generic;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Controllers;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class WinDashboardDataSourceController : WinCustomizeDashboardViewerControllerBase
	{
		// Extracts configured during ConfigureDataConnection are collected here and
		// consumed by the corresponding DashboardLoaded event to call PatchDashboard.
		private readonly List<IDashboardDataExtract> viewerExtracts = new List<IDashboardDataExtract>();
		private readonly List<IDashboardDataExtract> designerExtracts = new List<IDashboardDataExtract>();

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
			if (extract != null)
				designerExtracts.Add(extract);
		}

		private void DashboardDesigner_DashboardLoaded(object sender, DashboardLoadedEventArgs e)
		{
			var dashboard = ((DashboardDesigner)sender).Dashboard;
			PatchExtracts(designerExtracts, dashboard, designMode: true);
			CustomizeDashboard(dashboard, designMode: true);
		}

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
			if (extract != null)
				viewerExtracts.Add(extract);
		}

		private void DashboardViewer_DashboardLoaded(object sender, DashboardLoadedEventArgs e)
		{
			var dashboard = ((DashboardViewer)sender).Dashboard;
			PatchExtracts(viewerExtracts, dashboard, designMode: false);
			CustomizeDashboard(dashboard, designMode: false);
		}

		private void PatchExtracts(List<IDashboardDataExtract> extracts, Dashboard dashboard, bool designMode)
		{
			foreach (var extract in extracts)
				SenDevDashboardsModule.BackendRegistry.GetBackend(extract.BackendType)?.PatchDashboard(dashboard, extract, designMode);
			extracts.Clear();
		}

		private void CustomizeDashboard(Dashboard dashboard, bool designMode)
		{
			var customizeDashboardController = Frame.GetController<CustomizeDashboardController>();
			if (customizeDashboardController != null && customizeDashboardController.ShouldCustomizeDashboard)
				customizeDashboardController.RaiseCustomizeDashboard(new CustomizeDashboardEventArgs(dashboard, designMode));
		}
	}
}

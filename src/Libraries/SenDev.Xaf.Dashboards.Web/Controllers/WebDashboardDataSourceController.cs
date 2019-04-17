using System;
using System.Xml.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Controllers;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebDashboardDataSourceController : ObjectViewController<DetailView, IDashboardData>
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

		private WebDashboardViewerViewItem GetDashboardViewItem()
		{
			return View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
		}

		private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
		{
			CustomizeDashboardControl(((WebDashboardViewerViewItem)sender).DashboardControl);
		}
		private void CustomizeDashboardControl(ASPxDashboard dashboard)
		{
			dashboard.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
			dashboard.DashboardLoading += Dashboard_DashboardLoading;
		}

		private void Dashboard_DashboardLoading(object sender, DashboardLoadingWebEventArgs e)
		{
			var customizeDashboardController = Frame.GetController<CustomizeDashboardController>();
			if (customizeDashboardController != null && customizeDashboardController.ShouldCustomizeDashboard)
			{
				Dashboard dashboard = new Dashboard();
				dashboard.LoadFromXDocument(e.DashboardXml);
				customizeDashboardController.RaiseCustomizeDashboard(
					new CustomizeDashboardEventArgs(dashboard, View.ViewEditMode == DevExpress.ExpressApp.Editors.ViewEditMode.Edit));
				e.DashboardXml = dashboard.SaveToXDocument();
			}
		}


		private void DashboardViewer_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
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
			var dashboardViewerViewItem = GetDashboardViewItem();
			if (dashboardViewerViewItem != null)
			{
				dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
			}
			base.OnDeactivated();
		}
	}

}


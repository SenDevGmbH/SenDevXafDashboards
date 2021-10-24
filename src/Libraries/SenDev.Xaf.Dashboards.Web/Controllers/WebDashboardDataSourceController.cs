using System;
using System.Xml.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Controllers;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebDashboardDataSourceController : WebDashboardControllerBase
	{
	
		
		protected override void CustomizeDashboardControl(ASPxDashboard dashboard)
		{
			dashboard.ConfigureDataConnection += Dashboard_ConfigureDataConnection;
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


		private void Dashboard_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
		{
			if (e.ConnectionParameters is ExtractDataSourceConnectionParameters extractParameters)
			{
				if (Guid.TryParse(extractParameters.FileName, out var id))
				{
					IDashboardDataExtract extract = GetDataExtract(id);
					if (extract != null)
						extract.ConfigureConnectionParameters(extractParameters);
				}
			}
		}

		protected virtual IDashboardDataExtract GetDataExtract(Guid id)
		{
			return (IDashboardDataExtract)ObjectSpace.GetObjectByKey(SenDevDashboardsModule.GetDashboardDataExtractType(Application), id);
		}

	}
}


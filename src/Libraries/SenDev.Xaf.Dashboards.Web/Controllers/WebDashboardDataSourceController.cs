using System;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebDashboardDataSourceController : WebCustomizeDashboardViewerControllerBase
	{

		protected override void CustomizeDashboardControl(ASPxDashboard dashboard)
		{
			dashboard.ConfigureDataConnection += DashboardViewer_ConfigureDataConnection;
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

	}

}


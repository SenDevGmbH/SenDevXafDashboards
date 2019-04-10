using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardWeb;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class WebSaveParametersController : WebCustomizeDashboardViewerControllerBase
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			if (SaveDashboardParametersEnabled)
				View.Closing += View_Closing;
		}


		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			View.Closing += View_Closing;
		}

		private bool SaveDashboardParametersEnabled
		{
			get
			{
				if (Application.Model.Options is IModelOptionsXtraDashboards options)
					return options.XtraDashboards.SaveDashboardParameters;
				else
					return false;
			}
		}

		private void View_Closing(object sender, EventArgs e)
		{
			//var dashboardControl = FindDashboardViewerItem()?.DashboardControl;
			//if (dashboardControl != null)
			//	DashboardHelper.SaveParameters(Application, GetDashboardId(), dashboardControl.das);
		}

		protected override void CustomizeDashboardControl(ASPxDashboard dashboardControl)
		{
			if (SaveDashboardParametersEnabled)
				dashboardControl.DashboardAdding += DashboardControl_DashboardAdding;
		}

		private void DashboardControl_DashboardAdding(object sender, DashboardAddingWebEventArgs e)
		{
		}

		private void DashboardViewer_DashboardChanged(object sender, EventArgs e)
		{
			
			//DashboardHelper.RestoreParameters(Application, GetDashboardId(), ((ASPxDashboard)sender).Dashboard);
		}


		private string GetDashboardId()
		{
			return ObjectSpace.GetKeyValueAsString(ViewCurrentObject);
		}
	}
}

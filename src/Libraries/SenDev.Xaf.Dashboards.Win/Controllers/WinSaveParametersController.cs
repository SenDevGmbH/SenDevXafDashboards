using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardWin;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Win.Controllers
{
	public class WinSaveParametersController : WinCustomizeDashboardViewerControllerBase
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
			var dashboardViewer = FindDashboardViewerItem()?.Viewer;
			if (dashboardViewer != null)
				DashboardHelper.SaveParameters(Application, GetDashboardId(), dashboardViewer.Parameters);
		}

		protected override void CustomizeDashboardViewer(DashboardViewer dashboardViewer)
		{
			if (SaveDashboardParametersEnabled)
				dashboardViewer.DashboardChanged += DashboardViewer_DashboardChanged;
		}

		private void DashboardViewer_DashboardChanged(object sender, EventArgs e)
		{
			DashboardHelper.RestoreParameters(Application, GetDashboardId(), ((DashboardViewer)sender).Parameters);
		}


		private string GetDashboardId()
		{
			return ObjectSpace.GetKeyValueAsString(ViewCurrentObject);
		}
	}
}

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
			View.Closing += View_Closing;
		}

		private void View_Closing(object sender, EventArgs e)
		{
			var dashboardViewer = FindDashboardViewerItem()?.Viewer;
			if (dashboardViewer != null)
				DashboardHelper.SaveParameters(Application, GetDashboardId(), dashboardViewer.Parameters);
		}

		protected override void CustomizeDashboardViewer(DashboardViewer dashboardViewer)
		{
			//dashboardViewer.CustomParameters += DashboardViewer_CustomParameters;
			dashboardViewer.DashboardChanged += DashboardViewer_DashboardChanged;
		}

		private void DashboardViewer_DashboardChanged(object sender, EventArgs e)
		{
			DashboardHelper.RestoreParameters(Application, GetDashboardId(), ((DashboardViewer)sender).Parameters);
		}

		private void DashboardViewer_CustomParameters(object sender, DevExpress.DashboardCommon.CustomParametersEventArgs e)
		{
			DashboardHelper.RestoreParameters(Application, GetDashboardId(), e.Parameters);

		}

		private string GetDashboardId()
		{
			return ObjectSpace.GetKeyValueAsString(ViewCurrentObject);
		}
	}
}

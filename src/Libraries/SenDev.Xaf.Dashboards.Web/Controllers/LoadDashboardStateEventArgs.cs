using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class LoadDashboardStateEventArgs : HandledEventArgsBase
	{
		public string StateJson
		{
			get;set;
		}
	}

}



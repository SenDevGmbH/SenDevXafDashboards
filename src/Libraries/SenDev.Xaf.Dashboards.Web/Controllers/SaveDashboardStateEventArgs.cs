using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Web.Controllers
{
	public class SaveDashboardStateEventArgs : HandledEventArgsBase
	{
		public SaveDashboardStateEventArgs(string stateJson)
		{
			StateJson = stateJson;
		}

		public string StateJson
		{
			get;
		}
	}

}



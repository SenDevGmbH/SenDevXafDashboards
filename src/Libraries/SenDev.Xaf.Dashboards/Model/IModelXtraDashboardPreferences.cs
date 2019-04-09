using DevExpress.ExpressApp.Model;

namespace SenDev.Xaf.Dashboards
{
	public interface IModelXtraDashboardPreferences : IModelNode
	{
		IModelDashboardParametersList Parameters
		{
			get;
		}
	}
}

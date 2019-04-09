using DevExpress.ExpressApp.Model;

namespace SenDev.Xaf.Dashboards
{
	public interface IModelDashboardsList : IModelNode, IModelList<IModelXtraDashboardPreferences>
	{
		bool SaveDashboardParameters
		{
			get; set;
		}
	}
}

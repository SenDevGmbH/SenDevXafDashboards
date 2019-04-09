using DevExpress.ExpressApp.Model;

namespace SenDev.Xaf.Dashboards
{
	public interface IModelDashboardParameter : IModelNode
	{
		object Value
		{
			get;
			set;
		}
	}
}

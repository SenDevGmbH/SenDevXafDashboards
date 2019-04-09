using DevExpress.ExpressApp.Model;

namespace SenDev.Xaf.Dashboards
{
	public interface IModelDashboardParameter : IModelNode
	{
		string Value
		{
			get;
			set;
		}
	}
}

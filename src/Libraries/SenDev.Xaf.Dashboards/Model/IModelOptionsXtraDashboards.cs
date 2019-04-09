using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;

namespace SenDev.Xaf.Dashboards
{
	public interface IModelOptionsXtraDashboards : IModelNode
	{
		IModelDashboardsList XtraDashboards
		{
			get;
		}
	}
}

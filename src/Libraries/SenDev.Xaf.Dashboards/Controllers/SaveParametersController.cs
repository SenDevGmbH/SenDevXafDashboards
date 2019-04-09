using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class SaveParametersController : ObjectViewController<ObjectView, IDashboardData>, IModelExtender
	{
		public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
		{
			extenders.Add<IModelOptions, IModelOptionsXtraDashboards>();
		}
	}
}

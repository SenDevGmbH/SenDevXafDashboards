using System;
using System.Linq;
using DevExpress.DataProcessing;
using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class DataExtractJobSchedulerController : ObjectViewController<DetailView, DashboardDataExtract>
	{
		protected override void OnActivated()
		{
			base.OnActivated();
			ObjectSpace.Committed += ObjectSpace_Committed;
			ObjectSpace.ObjectDeleted += ObjectSpace_ObjectDeleted;
		}

		private void ObjectSpace_ObjectDeleted(object sender, ObjectsManipulatingEventArgs e)
		{
			if (JobScheduler != null)
				e.Objects.OfType<DashboardDataExtract>().ForEach(de => JobScheduler.RemoveDataExtractCreationJob(de));
		}

		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			ObjectSpace.Committed -= ObjectSpace_Committed;
			ObjectSpace.ObjectDeleted -= ObjectSpace_ObjectDeleted;

		}
		private void ObjectSpace_Committed(object sender, EventArgs e)
		{
			JobScheduler?.ScheduleDataExtractCreationJob(ViewCurrentObject);
		}

		public IJobScheduler JobScheduler => Application.Modules.FindModule<SenDevDashboardsModule>()?.JobScheduler;
	}
}

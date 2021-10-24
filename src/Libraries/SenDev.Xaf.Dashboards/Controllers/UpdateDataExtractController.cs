using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SenDev.Xaf.Dashboards.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;
using SenDev.Xaf.Dashboards.Utils;

namespace SenDev.Xaf.Dashboards.Controllers
{
	public class UpdateDataExtractController : ObjectViewController<ListView, IDashboardDataExtract>
	{


		public UpdateDataExtractController()
		{
			UpdateDataExtractAction = new SimpleAction(this, "UpdateDataExtract", PredefinedCategory.Edit);
			UpdateDataExtractAction.Caption = "Update data";
			UpdateDataExtractAction.ImageName = "Action_SimpleAction";
			UpdateDataExtractAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
			UpdateDataExtractAction.Execute += UpdateDataExtractAction_Execute;
			UpdateDataExtractAction.ConfirmationMessage = "Caution: This action can take very long time and it cannot be cancelled, are You sure to proceed?";
		}

        private bool UpdateBySchedulerEnabled
        {
            get
            {
                if (Application.Model.Options is IModelOptionsXtraDashboards options)
                    return options.XtraDashboards.UseSchedulerToCreateDataExtractsFromUI;
                else
                    return false;
            }
        }
        private void UpdateDataExtractAction_Execute(object sender, SimpleActionExecuteEventArgs e)
		{
            var scheduler = Application.GetJobScheduler();
			if (scheduler != null && UpdateBySchedulerEnabled)
			{
				scheduler.StartDataExtractUpdate(ViewCurrentObject);
			}
			else
			{
				var dm = new DataExtractDataManager(Application);
				dm.UpdateDataExtractByKey(ObjectSpace.GetKeyValue(ViewCurrentObject));
				ObjectSpace.Refresh();
			}
		}

		public SimpleAction UpdateDataExtractAction
		{
			get;
		}
	}
}

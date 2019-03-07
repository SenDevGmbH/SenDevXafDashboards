using DevExpress.ExpressApp;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages
{

	public interface IEnterScriptPageView
	{
		ScriptDashboardWizardParameters WizardParameters
		{
			get; set;
		}
		IObjectSpace ObjectSpace
		{
			get; set;
		}
	}
}

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.ScriptingPages
{
	public class EnterScriptPageView : DashboardDataSourceWizardViewBase, IEnterScriptPageView
	{
		public EnterScriptPageView(ScriptDashboardWizardParameters parameters, IObjectSpace objectSpace, XafApplication application) : base(parameters, objectSpace, application)
		{
		}

		protected override DetailView CreateDetailView(IObjectSpace objectSpace)
		{
			return Application.CreateDetailView(objectSpace, WizardParameters);
		}
		public override string HeaderDescription
		{
			get
			{
				return CaptionHelper.GetLocalizedText("Captions", "EnterScript");
			}
		}
	}
}

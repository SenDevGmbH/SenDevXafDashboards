using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.DataExtractPages
{
	public class ChoiceDataExtractPageView : DashboardDataSourceWizardViewBase, IChoiceDataExctractPageView
	{
		public ChoiceDataExtractPageView(ScriptDashboardWizardParameters parameters, IObjectSpace objectSpace, XafApplication application) : base(parameters, objectSpace, application)
		{
		}

		protected override DetailView CreateDetailView(IObjectSpace objectSpace)
		{
			return Application.CreateDetailView(objectSpace, "ScriptDashboardWizardParameters_DetailView_Extract", true, WizardParameters);
		}
		public override string HeaderDescription => CaptionHelper.GetLocalizedText("Captions", "EnterScript");
	}
}

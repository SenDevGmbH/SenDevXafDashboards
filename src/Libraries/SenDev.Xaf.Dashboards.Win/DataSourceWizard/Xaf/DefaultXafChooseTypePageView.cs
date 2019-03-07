using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.ExpressApp.Utils;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard.Xaf
{
	public class DefaultXafChooseTypePageView : DashboardDataSourceWizardViewBase, IChooseXafTypePageView
	{
		public DefaultXafChooseTypePageView(BusinessObjects.ScriptDashboardWizardParameters parameters, IObjectSpace objectSpace, XafApplication application)
			: base(parameters, objectSpace, application)
		{
		}

		public override string HeaderDescription => CaptionHelper.GetLocalizedText("Captions", "ChooseXafType");

		protected override DetailView CreateDetailView(IObjectSpace objectSpace) => Application.CreateDetailView(objectSpace, "XafWizardParameters_DetailView", true, WizardParameters);
		XafWizardParameters IChooseXafTypePageView.WizardParameters
		{
			get => WizardParameters;
			set => WizardParameters = (ScriptDashboardWizardParameters)value;
		}

	}

}

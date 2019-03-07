using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Win.BusinessObjects;

namespace SenDev.Xaf.Dashboards.Win.DataSourceWizard
{

	public class RibbonDashboardDesignerCreatorEx : RibbonDashboardDesignerCreator
	{
		public RibbonDashboardDesignerCreatorEx(XafApplication application) : base(application)
		{
			Application = application;
		}

		private XafApplication Application
		{
			get;
		}

		protected override XafDashboardDataSourceWizardCustomization CreateXafDataSourceWizardCustomization()
		{
			var parameters = new ScriptDashboardWizardParameters();
			IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(XafWizardParameters));
			return new DashboardCustomization(parameters, objectSpace, Application);
		}
	}
}

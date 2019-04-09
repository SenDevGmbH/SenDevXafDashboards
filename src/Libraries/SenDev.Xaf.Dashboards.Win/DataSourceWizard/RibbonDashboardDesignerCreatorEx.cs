using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using SenDev.Xaf.Dashboards.Scripting;
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
			var defaultType = Application.Modules.FindModule<SenDevDashboardsModule>()?.DefaultBusinessObjectType;
			if (defaultType != null)
				parameters.Script = TemplateHelper.GetScriptTemplate(defaultType);

			IObjectSpace objectSpace = Application.CreateObjectSpace();
			return new DashboardCustomization(parameters, objectSpace, Application);
		}
	}
}

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
			SenDevDashboardsModule module = Application.Modules.FindModule<SenDevDashboardsModule>();
			IObjectSpace objectSpace = Application.CreateObjectSpace();

			var parameters = new ScriptDashboardWizardParameters(objectSpace, module?.DashboardExtractType);
			var defaultType = module?.DefaultBusinessObjectType;
			if (defaultType != null)
				parameters.Script = TemplateHelper.GetScriptTemplate(defaultType);

			return new DashboardCustomization(parameters, objectSpace, Application);
		}
	}
}
